import { Injectable, signal, computed, effect } from '@angular/core';

export type MicrophoneStatus = 'idle' | 'listening' | 'recording' | 'processing';
export type RecordingState = 'idle' | 'active' | 'paused' | 'stopped';

export interface AnalyticsMetrics {
  recordingDuration: number; // seconds
  wordCount: number;
  speakingSpeed: number; // words per minute
  currentTranscript: string;
  microphoneStatus: MicrophoneStatus;
  silenceCount: number;
  recordingState: RecordingState;
  lastSilenceDuration: number; // seconds
  isLongPause: boolean; // pause > 3 seconds
}

@Injectable({
  providedIn: 'root'
})
export class InterviewAnalyticsService {
  // Core reactive state
  private readonly recordingDuration = signal(0); // seconds
  private readonly wordCount = signal(0);
  private readonly currentTranscript = signal('');
  private readonly microphoneStatus = signal<MicrophoneStatus>('idle');
  private readonly silenceCount = signal(0);
  private readonly recordingState = signal<RecordingState>('idle');
  private readonly lastSilenceDuration = signal(0);
  private readonly isLongPause = signal(false);

  // Computed metrics
  readonly speakingSpeed = computed(() => {
    const duration = this.recordingDuration();
    if (duration === 0) return 0;
    const minutes = duration / 60;
    return Math.round(this.wordCount() / minutes);
  });

  // Public read-only signals
  readonly metrics = computed<AnalyticsMetrics>(() => ({
    recordingDuration: this.recordingDuration(),
    wordCount: this.wordCount(),
    speakingSpeed: this.speakingSpeed(),
    currentTranscript: this.currentTranscript(),
    microphoneStatus: this.microphoneStatus(),
    silenceCount: this.silenceCount(),
    recordingState: this.recordingState(),
    lastSilenceDuration: this.lastSilenceDuration(),
    isLongPause: this.isLongPause()
  }));

  // Private state for internal tracking
  private timerInterval?: any;
  private silenceTimer?: any;
  private silenceThreshold = 3000; // 3 seconds in milliseconds
  private lastTranscriptUpdate = Date.now();
  private speechRecognition?: any;
  private isRecognitionActive = false;

  constructor() {
    // Auto-cleanup effect
    effect(() => {
      const state = this.recordingState();
      if (state === 'stopped' || state === 'idle') {
        this.stopSilenceDetection();
      }
    });
  }

  /**
   * Start recording analytics tracking
   */
  startRecording(): void {
    this.recordingState.set('active');
    this.microphoneStatus.set('recording');
    this.recordingDuration.set(0);
    this.wordCount.set(0);
    this.currentTranscript.set('');
    this.silenceCount.set(0);
    this.lastSilenceDuration.set(0);
    this.isLongPause.set(false);

    // Start timer
    this.startTimer();

    // Start silence detection
    this.startSilenceDetection();

    // Start browser speech recognition for live preview
    this.startLiveSpeechRecognition();
  }

  /**
   * Stop recording analytics tracking
   */
  stopRecording(): void {
    this.recordingState.set('stopped');
    this.microphoneStatus.set('processing');
    this.stopTimer();
    this.stopSilenceDetection();
    this.stopLiveSpeechRecognition();
  }

  /**
   * Reset all metrics to initial state
   */
  reset(): void {
    this.recordingState.set('idle');
    this.microphoneStatus.set('idle');
    this.recordingDuration.set(0);
    this.wordCount.set(0);
    this.currentTranscript.set('');
    this.silenceCount.set(0);
    this.lastSilenceDuration.set(0);
    this.isLongPause.set(false);
    this.stopTimer();
    this.stopSilenceDetection();
    this.stopLiveSpeechRecognition();
  }

  /**
   * Update microphone status manually
   */
  setMicrophoneStatus(status: MicrophoneStatus): void {
    this.microphoneStatus.set(status);
  }

  /**
   * Update transcript and recalculate word count
   */
  updateTranscript(transcript: string): void {
    this.currentTranscript.set(transcript);
    this.updateWordCount(transcript);
    this.lastTranscriptUpdate = Date.now();
    
    // Reset silence detection
    if (this.recordingState() === 'active') {
      this.resetSilenceTimer();
    }
  }

  /**
   * Get formatted recording duration as MM:SS
   */
  getFormattedDuration(): string {
    const duration = this.recordingDuration();
    const minutes = Math.floor(duration / 60);
    const seconds = duration % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }

  /**
   * Check if currently recording
   */
  isRecording(): boolean {
    return this.recordingState() === 'active';
  }

  // Private methods

  private startTimer(): void {
    this.stopTimer();
    this.timerInterval = setInterval(() => {
      this.recordingDuration.update(d => d + 1);
    }, 1000);
  }

  private stopTimer(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
      this.timerInterval = undefined;
    }
  }

  private startSilenceDetection(): void {
    this.resetSilenceTimer();
  }

  private stopSilenceDetection(): void {
    if (this.silenceTimer) {
      clearTimeout(this.silenceTimer);
      this.silenceTimer = undefined;
    }
    this.isLongPause.set(false);
  }

  private resetSilenceTimer(): void {
    this.isLongPause.set(false);
    
    if (this.silenceTimer) {
      clearTimeout(this.silenceTimer);
    }

    this.silenceTimer = setTimeout(() => {
      if (this.recordingState() === 'active') {
        this.onSilenceDetected();
      }
    }, this.silenceThreshold);
  }

  private onSilenceDetected(): void {
    this.silenceCount.update(c => c + 1);
    this.isLongPause.set(true);
    
    // Calculate silence duration
    const timeSinceLastUpdate = Date.now() - this.lastTranscriptUpdate;
    this.lastSilenceDuration.set(Math.floor(timeSinceLastUpdate / 1000));

    // Continue monitoring for more silence
    this.resetSilenceTimer();
  }

  private updateWordCount(transcript: string): void {
    if (!transcript || transcript.trim().length === 0) {
      this.wordCount.set(0);
      return;
    }

    // Count words by splitting on whitespace and filtering empty strings
    const words = transcript
      .trim()
      .split(/\s+/)
      .filter(word => word.length > 0);
    
    this.wordCount.set(words.length);
  }

  private startLiveSpeechRecognition(): void {
    // Check browser support
    const SpeechRecognition = (window as any).SpeechRecognition || (window as any).webkitSpeechRecognition;
    
    if (!SpeechRecognition) {
      console.warn('Browser does not support Speech Recognition API');
      return;
    }

    try {
      this.speechRecognition = new SpeechRecognition();
      this.speechRecognition.continuous = true;
      this.speechRecognition.interimResults = true;
      this.speechRecognition.lang = 'en-US';

      this.speechRecognition.onstart = () => {
        this.isRecognitionActive = true;
        console.log('Live speech recognition started');
      };

      this.speechRecognition.onresult = (event: any) => {
        let interimTranscript = '';
        let finalTranscript = '';

        for (let i = event.resultIndex; i < event.results.length; i++) {
          const transcript = event.results[i][0].transcript;
          if (event.results[i].isFinal) {
            finalTranscript += transcript + ' ';
          } else {
            interimTranscript += transcript;
          }
        }

        // Combine all previous final results with current interim
        const fullTranscript = this.currentTranscript() + finalTranscript + interimTranscript;
        this.updateTranscript(fullTranscript.trim());
      };

      this.speechRecognition.onerror = (event: any) => {
        console.error('Speech recognition error:', event.error);
        
        // Auto-restart on certain errors
        if (event.error === 'no-speech' || event.error === 'audio-capture') {
          setTimeout(() => {
            if (this.recordingState() === 'active' && !this.isRecognitionActive) {
              this.startLiveSpeechRecognition();
            }
          }, 1000);
        }
      };

      this.speechRecognition.onend = () => {
        this.isRecognitionActive = false;
        
        // Auto-restart if still recording
        if (this.recordingState() === 'active') {
          setTimeout(() => {
            this.startLiveSpeechRecognition();
          }, 100);
        }
      };

      this.speechRecognition.start();
      this.microphoneStatus.set('listening');
    } catch (error) {
      console.error('Failed to start speech recognition:', error);
    }
  }

  private stopLiveSpeechRecognition(): void {
    if (this.speechRecognition && this.isRecognitionActive) {
      try {
        this.speechRecognition.stop();
        this.isRecognitionActive = false;
      } catch (error) {
        console.error('Error stopping speech recognition:', error);
      }
    }
    this.speechRecognition = undefined;
  }

  /**
   * Extension point for future metrics
   * Future modules can add metrics here without changing existing code
   */
  extendMetrics<T extends Record<string, any>>(additionalMetrics: T): AnalyticsMetrics & T {
    return {
      ...this.metrics(),
      ...additionalMetrics
    };
  }
}
