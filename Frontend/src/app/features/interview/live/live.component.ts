import { Component, ElementRef, inject, OnDestroy, OnInit, PLATFORM_ID, signal, ViewChild } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { TitleCasePipe } from '@angular/common';
import { InterviewService } from '../../../core/services/interview.service';
import { InterviewAnalyticsService } from '../../../core/services/interview-analytics.service';
import { AnalyticsPanelComponent } from '../components/analytics-panel/analytics-panel.component';
import { InterviewSession, InterviewQuestion, SubmitAudioAnswerResponse } from '../../../core/models/interview.models';

type SpeechState = 'idle' | 'ai-speaking' | 'user-turn' | 'user-recording';

@Component({
  selector: 'app-live-interview',
  standalone: true,
  imports: [TitleCasePipe, AnalyticsPanelComponent],
  templateUrl: './live.component.html',
  styleUrl: './live.component.scss'
})
export class LiveComponent implements OnInit, OnDestroy {
  @ViewChild('cameraPreview') cameraPreview?: ElementRef<HTMLVideoElement>;

  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly interviewService = inject(InterviewService);
  private readonly analytics = inject(InterviewAnalyticsService);

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

    // Load session from memory first
    let session = this.interviewService.getCurrentSession();
    
    if (!session || session.id !== this.sessionId) {
      // Session not in memory, load from backend
      this.loading.set(true);
      this.interviewService.loadSessionDetails(this.sessionId).subscribe({
        next: (loadedSession) => {
          this.session.set(loadedSession);
          this.initializeSession();
        },
        error: () => {
          this.error.set('Session not found');
          this.router.navigate(['/dashboard']);
        }
      });
    } else {
      // Session exists in memory
      this.session.set(session);
      
      // Load questions if not already loaded
      if (session.questions.length === 0) {
        this.loading.set(true);
        this.interviewService.loadQuestions(this.sessionId).subscribe({
          next: (questions) => {
            const updatedSession = this.interviewService.getCurrentSession();
            if (updatedSession) {
              this.session.set(updatedSession);
              this.initializeSession();
            }
          },
          error: () => {
            this.error.set('Failed to load questions');
            this.loading.set(false);
          }
        });
      } else {
        this.initializeSession();
      }
    }
  }

  private initializeSession(): void {
    this.loading.set(false);
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
      // Release any existing stream first
      this.releaseMediaStream();

      this.mediaStream = await navigator.mediaDevices.getUserMedia({ 
        audio: {
          echoCancellation: true,
          noiseSuppression: true,
          sampleRate: 44100
        } 
      });

      // Start analytics tracking with media stream for voice energy analysis
      this.analytics.startRecording(this.mediaStream);
      
      this.audioChunks = [];

      // Try multiple MIME types for better compatibility
      let mimeType = 'audio/webm;codecs=opus';
      if (!MediaRecorder.isTypeSupported(mimeType)) {
        mimeType = 'audio/webm';
        if (!MediaRecorder.isTypeSupported(mimeType)) {
          mimeType = 'audio/mp4';
          if (!MediaRecorder.isTypeSupported(mimeType)) {
            mimeType = ''; // Use default
          }
        }
      }

      const options = mimeType ? { mimeType } : {};
      this.mediaRecorder = new MediaRecorder(this.mediaStream, options);

      this.mediaRecorder.ondataavailable = (event) => {
        if (event.data && event.data.size > 0) {
          this.audioChunks.push(event.data);
        }
      };

      this.mediaRecorder.onstop = () => {
        this.processRecording();
      };

      this.mediaRecorder.onerror = (event: any) => {
        console.error('MediaRecorder error:', event);
        this.error.set('Recording error occurred. Please try again.');
        this.speechState.set('user-turn');
        this.analytics.stopRecording();
        this.releaseMediaStream();
      };

      this.mediaRecorder.start(100); // Collect data every 100ms
      this.speechState.set('user-recording');
      this.error.set('');
    } catch (err: any) {
      console.error('Microphone access error:', err);
      let errorMessage = 'Could not access microphone. ';

      if (err.name === 'NotAllowedError') {
        errorMessage += 'Please allow microphone permissions.';
      } else if (err.name === 'NotFoundError') {
        errorMessage += 'No microphone found.';
      } else if (err.name === 'NotReadableError') {
        errorMessage += 'Microphone is being used by another application.';
      } else {
        errorMessage += 'Please check your settings and try again.';
      }

      this.error.set(errorMessage);
      this.speechState.set('user-turn');
      this.analytics.reset();
      this.releaseMediaStream();
    }
  }

  stopRecording(): void {
    // Stop analytics tracking
    this.analytics.stopRecording();

    if (this.mediaRecorder && this.mediaRecorder.state === 'recording') {
      try {
        this.mediaRecorder.stop();
      } catch (err) {
        console.error('Error stopping recorder:', err);
        this.processRecording(); // Try to process anyway
      }
    } else if (this.mediaRecorder && this.mediaRecorder.state === 'inactive') {
      // Already stopped, process if we have chunks
      if (this.audioChunks.length > 0) {
        this.processRecording();
      }
    }
  }

  private processRecording(): void {
    if (this.audioChunks.length === 0) {
      this.error.set('No audio recorded. Please try again.');
      this.speechState.set('user-turn');
      this.releaseMediaStream();
      return;
    }

    const audioBlob = new Blob(this.audioChunks, { type: 'audio/webm' });
    const session = this.session();
    const question = this.currentQuestion();

    if (!session || !question) return;

    // Calculate duration (approximate)
    const durationSeconds = Math.max(1, Math.floor(this.audioChunks.length / 10));

    // Show transcribing state
    this.currentTranscript.set('Transcribing...');
    this.speechState.set('user-turn');

    // Submit audio to backend for transcription ONLY (no evaluation)
    this.interviewService.submitAudioAnswer(
      session.id,
      question.id,
      audioBlob,
      durationSeconds
    ).subscribe({
      next: (response) => {
        // Show transcript immediately
        this.currentTranscript.set(response.transcript);

        // Update session state
        const updatedSession = this.interviewService.getCurrentSession();
        if (updatedSession) {
          this.session.set(updatedSession);
        }

        this.releaseMediaStream();
        this.audioChunks = [];
      },
      error: (err) => {
        this.currentTranscript.set('');
        this.error.set('Failed to transcribe audio. Please try recording again.');
        console.error('Audio submission error:', err);
        this.releaseMediaStream();
        this.audioChunks = [];
      }
    });
  }

  private releaseMediaStream(): void {
    if (this.mediaStream) {
      this.mediaStream.getTracks().forEach(track => {
        track.stop();
        track.enabled = false;
      });
      this.mediaStream = undefined;
    }
    this.mediaRecorder = undefined;
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
    this.analytics.reset(); // Reset analytics for next question

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
    this.analytics.reset(); // Reset analytics for previous question

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
    this.error.set('');

    const session = this.session();
    if (!session) return;

    // Complete the interview via backend API
    // Backend will evaluate all answers in batch and generate results
    this.interviewService.completeInterview(session.id).subscribe({
      next: () => {
        // Navigate to results page (results will be fetched there)
        this.router.navigate(['/interview/result', session.id]);
      },
      error: (err) => {
        console.error('Failed to complete interview:', err);
        this.error.set('Failed to complete interview. Please try again.');
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
