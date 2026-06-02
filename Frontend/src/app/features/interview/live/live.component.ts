import { Component, inject, signal, OnInit, OnDestroy, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { TitleCasePipe } from '@angular/common';
import { InterviewService } from '../../../core/services/interview.service';
import { InterviewHistoryService } from '../../../core/services/interview-history.service';
import { SpeechTranscriptionService } from '../../../core/services/speech-transcription.service';
import { InterviewSession, InterviewQuestion } from '../../../core/models/interview.models';

type SpeechState = 'idle' | 'ai-speaking' | 'user-turn' | 'user-recording';

@Component({
  selector: 'app-live-interview',
  standalone: true,
  imports: [TitleCasePipe],
  templateUrl: './live.component.html',
  styleUrl: './live.component.scss'
})
export class LiveComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly interviewService = inject(InterviewService);
  private readonly historyService = inject(InterviewHistoryService);
  private readonly transcriptionService = inject(SpeechTranscriptionService);

  readonly session = signal<InterviewSession | null>(null);
  readonly currentQuestion = signal<InterviewQuestion | null>(null);
  readonly timeRemaining = signal(0);
  readonly loading = signal(false);
  readonly error = signal('');
  
  // Speech & Recording State
  readonly speechState = signal<SpeechState>('idle');
  readonly showCaptions = signal(true);
  readonly currentTranscript = signal('');
  
  private sessionId = '';
  private timerInterval?: any;
  private speechSynthesis?: SpeechSynthesis;
  private currentUtterance?: SpeechSynthesisUtterance;
  private mediaRecorder?: MediaRecorder;
  private audioChunks: BlobPart[] = [];
  private mediaStream?: MediaStream;

  ngOnInit(): void {
    if (!isPlatformBrowser(this.platformId)) return;

    this.sessionId = this.route.snapshot.paramMap.get('sessionId') || '';
    if (!this.sessionId) {
      this.router.navigate(['/dashboard']);
      return;
    }

    const session = this.interviewService.getCurrentSession();
    if (!session || session.id !== this.sessionId) {
      this.router.navigate(['/dashboard']);
      return;
    }

    this.session.set(session);
    this.updateCurrentQuestion();
    this.startTimer();
    this.loadExistingTranscript();
    
    // Initialize Speech Synthesis
    if ('speechSynthesis' in window) {
      this.speechSynthesis = window.speechSynthesis;
    }

    // Auto-speak first question
    setTimeout(() => this.speakQuestion(), 500);
  }

  ngOnDestroy(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
    this.stopSpeaking();
    this.stopRecording();
    this.releaseMediaStream();
  }

  private startTimer(): void {
    const session = this.session();
    if (!session) return;

    const totalSeconds = session.setup.duration * 60;
    this.timeRemaining.set(totalSeconds);

    this.timerInterval = setInterval(() => {
      const remaining = this.timeRemaining();
      if (remaining > 0) {
        this.timeRemaining.set(remaining - 1);
      } else {
        clearInterval(this.timerInterval);
        this.finishInterview();
      }
    }, 1000);
  }

  private updateCurrentQuestion(): void {
    const session = this.session();
    if (!session) return;

    const question = session.questions[session.currentQuestionIndex];
    this.currentQuestion.set(question || null);
  }

  private loadExistingTranscript(): void {
    const session = this.session();
    const question = this.currentQuestion();
    if (!session || !question) return;

    const existingAnswer = session.answers.find(a => a.questionId === question.id);
    if (existingAnswer) {
      this.currentTranscript.set(existingAnswer.text);
    } else {
      this.currentTranscript.set('');
    }
  }

  // ──────────────────────────────────────────────────────────────
  // Speech Synthesis
  // ──────────────────────────────────────────────────────────────

  speakQuestion(): void {
    if (!isPlatformBrowser(this.platformId) || !this.speechSynthesis) return;
    
    const question = this.currentQuestion();
    if (!question) return;

    this.stopSpeaking();
    this.speechState.set('ai-speaking');

    this.currentUtterance = new SpeechSynthesisUtterance(question.text);
    this.currentUtterance.rate = 0.9;
    this.currentUtterance.pitch = 1.0;
    this.currentUtterance.volume = 1.0;

    this.currentUtterance.onend = () => {
      this.speechState.set('user-turn');
    };

    this.currentUtterance.onerror = () => {
      this.speechState.set('user-turn');
      this.error.set('Speech synthesis error. Please continue with text input.');
    };

    this.speechSynthesis.speak(this.currentUtterance);
  }

  stopSpeaking(): void {
    if (this.speechSynthesis && this.currentUtterance) {
      this.speechSynthesis.cancel();
    }
  }

  toggleCaptions(): void {
    this.showCaptions.set(!this.showCaptions());
  }

  // ──────────────────────────────────────────────────────────────
  // Voice Recording
  // ──────────────────────────────────────────────────────────────

  async startRecording(): Promise<void> {
    if (!isPlatformBrowser(this.platformId)) return;

    try {
      this.mediaStream = await navigator.mediaDevices.getUserMedia({ audio: true });
      this.audioChunks = [];

      const mimeType = MediaRecorder.isTypeSupported('audio/webm;codecs=opus')
        ? 'audio/webm;codecs=opus'
        : 'audio/webm';

      this.mediaRecorder = new MediaRecorder(this.mediaStream, { mimeType });

      this.mediaRecorder.ondataavailable = (event) => {
        if (event.data.size > 0) {
          this.audioChunks.push(event.data);
        }
      };

      this.mediaRecorder.onstop = () => {
        this.processRecording();
      };

      this.mediaRecorder.start();
      this.speechState.set('user-recording');
      this.error.set('');
    } catch (err) {
      this.error.set('Could not access microphone. Please check permissions.');
      this.speechState.set('user-turn');
    }
  }

  stopRecording(): void {
    if (this.mediaRecorder && this.mediaRecorder.state === 'recording') {
      this.mediaRecorder.stop();
    }
  }

  private processRecording(): void {
    if (this.audioChunks.length === 0) return;

    const audioBlob = new Blob(this.audioChunks, { type: 'audio/webm' });
    this.speechState.set('user-turn');

    // Transcribe using mock service
    this.transcriptionService.transcribe(audioBlob).subscribe({
      next: (result) => {
        const newTranscript = this.currentTranscript() 
          ? `${this.currentTranscript()} ${result.text}`
          : result.text;
        
        this.currentTranscript.set(newTranscript);
        this.saveTranscriptToSession();
      },
      error: () => {
        this.error.set('Transcription failed. Please try again.');
      }
    });

    this.releaseMediaStream();
  }

  private releaseMediaStream(): void {
    if (this.mediaStream) {
      this.mediaStream.getTracks().forEach(track => track.stop());
      this.mediaStream = undefined;
    }
  }

  private saveTranscriptToSession(): void {
    const session = this.session();
    const question = this.currentQuestion();
    if (!session || !question) return;

    const transcript = this.currentTranscript();
    if (!transcript.trim()) return;

    this.interviewService.saveTranscript(session.id, question.id, transcript).subscribe();
  }

  clearTranscript(): void {
    this.currentTranscript.set('');
    this.saveTranscriptToSession();
  }

  // ──────────────────────────────────────────────────────────────
  // Navigation
  // ──────────────────────────────────────────────────────────────

  nextQuestion(): void {
    this.saveTranscriptToSession();
    this.stopSpeaking();
    this.stopRecording();

    const session = this.session();
    if (!session) return;

    if (session.currentQuestionIndex < session.questions.length - 1) {
      const newIndex = session.currentQuestionIndex + 1;
      this.interviewService.updateQuestionIndex(session.id, newIndex).subscribe(() => {
        const updatedSession = this.interviewService.getCurrentSession();
        if (updatedSession) {
          this.session.set(updatedSession);
          this.updateCurrentQuestion();
          this.loadExistingTranscript();
          this.speechState.set('idle');
          setTimeout(() => this.speakQuestion(), 300);
        }
      });
    }
  }

  previousQuestion(): void {
    this.saveTranscriptToSession();
    this.stopSpeaking();
    this.stopRecording();

    const session = this.session();
    if (!session) return;

    if (session.currentQuestionIndex > 0) {
      const newIndex = session.currentQuestionIndex - 1;
      this.interviewService.updateQuestionIndex(session.id, newIndex).subscribe(() => {
        const updatedSession = this.interviewService.getCurrentSession();
        if (updatedSession) {
          this.session.set(updatedSession);
          this.updateCurrentQuestion();
          this.loadExistingTranscript();
          this.speechState.set('idle');
          setTimeout(() => this.speakQuestion(), 300);
        }
      });
    }
  }

  finishInterview(): void {
    this.saveTranscriptToSession();
    this.stopSpeaking();
    this.stopRecording();
    this.loading.set(true);

    const session = this.session();
    if (!session) return;

    this.interviewService.finishSession(session.id).subscribe({
      next: (result) => {
        this.historyService.saveSession(result).subscribe(() => {
          this.router.navigate(['/interview/result', session.id]);
        });
      },
      error: () => {
        this.error.set('Failed to finish interview. Please try again.');
        this.loading.set(false);
      }
    });
  }

  // ──────────────────────────────────────────────────────────────
  // UI Helpers
  // ──────────────────────────────────────────────────────────────

  get formattedTime(): string {
    const remaining = this.timeRemaining();
    const minutes = Math.floor(remaining / 60);
    const seconds = remaining % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }

  get isLastQuestion(): boolean {
    const session = this.session();
    if (!session) return false;
    return session.currentQuestionIndex === session.questions.length - 1;
  }

  get isFirstQuestion(): boolean {
    const session = this.session();
    if (!session) return false;
    return session.currentQuestionIndex === 0;
  }

  get canRecord(): boolean {
    const state = this.speechState();
    return state === 'user-turn' && !this.loading();
  }

  get isRecording(): boolean {
    return this.speechState() === 'user-recording';
  }

  get isAiSpeaking(): boolean {
    return this.speechState() === 'ai-speaking';
  }

  get statusText(): string {
    switch (this.speechState()) {
      case 'ai-speaking': return 'AI Speaking...';
      case 'user-recording': return 'Recording...';
      case 'user-turn': return 'Your Turn';
      default: return 'Ready';
    }
  }
}
