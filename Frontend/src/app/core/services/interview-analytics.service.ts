import { Injectable, signal, computed, effect } from '@angular/core';

export type MicrophoneStatus = 'idle' | 'listening' | 'recording' | 'processing';
export type RecordingState = 'idle' | 'active' | 'paused' | 'stopped';
export type PaceRating = 'Too Slow' | 'Good' | 'Excellent' | 'Too Fast';

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

export interface VoiceIntelligenceMetrics {
  // Speaking Pace Analysis
  averageWPM: number;
  paceRating: PaceRating;
  
  // Filler Word Detection
  fillerWordCount: number;
  mostUsedFillerWord: string;
  fillerWords: Map<string, number>;
  
  // Pause Analysis
  longestPause: number; // seconds
  pauseCount: number;
  averagePauseDuration: number; // seconds
  
  // Voice Energy
  voiceEnergy: number; // 0-100
  averageVolume: number; // 0-100
  volumeSamples: number[];
  
  // Fluency & Communication Scores
  fluencyScore: number; // 0-100
  communicationScore: number; // 0-100
}

export interface ExtendedAnalyticsMetrics extends AnalyticsMetrics {
  voiceIntelligence: VoiceIntelligenceMetrics;
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

  // Voice Intelligence State
  private readonly averageWPM = signal(0);
  private readonly paceRating = signal<PaceRating>('Good');
  private readonly fillerWordCount = signal(0);
  private readonly mostUsedFillerWord = signal('');
  private readonly fillerWordsMap = signal<Map<string, number>>(new Map());
  private readonly longestPause = signal(0);
  private readonly pauseCount = signal(0);
  private readonly averagePauseDuration = signal(0);
  private readonly voiceEnergy = signal(0);
  private readonly averageVolume = signal(0);
  private readonly volumeSamples = signal<number[]>([]);
  private readonly fluencyScore = signal(0);
  private readonly communicationScore = signal(0);

  // Computed metrics
  readonly speakingSpeed = computed(() => {
    if (this.recordingState() !== 'active') return 0;

    const duration = this.recordingDuration();
    const words = this.wordCount();

    if (words === 0 || duration < 3) return 0;

    const minutes = duration / 60;
    const wpm = Math.round(words / minutes);

    return Math.min(300, Math.max(0, wpm));
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

  // Voice Intelligence Metrics
  readonly voiceIntelligence = computed<VoiceIntelligenceMetrics>(() => ({
    averageWPM: this.averageWPM(),
    paceRating: this.paceRating(),
    fillerWordCount: this.fillerWordCount(),
    mostUsedFillerWord: this.mostUsedFillerWord(),
    fillerWords: this.fillerWordsMap(),
    longestPause: this.longestPause(),
    pauseCount: this.pauseCount(),
    averagePauseDuration: this.averagePauseDuration(),
    voiceEnergy: this.voiceEnergy(),
    averageVolume: this.averageVolume(),
    volumeSamples: this.volumeSamples(),
    fluencyScore: this.fluencyScore(),
    communicationScore: this.communicationScore()
  }));

  // Extended metrics combining both
  readonly extendedMetrics = computed<ExtendedAnalyticsMetrics>(() => ({
    ...this.metrics(),
    voiceIntelligence: this.voiceIntelligence()
  }));

  // Private state for internal tracking
  private timerInterval?: any;
  private silenceTimer?: any;
  private silenceThreshold = 3000; // 3 seconds in milliseconds
  private lastTranscriptUpdate = Date.now();
  private speechRecognition?: any;
  private isRecognitionActive = false;
  private audioContext?: AudioContext;
  private analyser?: AnalyserNode;
  private microphone?: MediaStreamAudioSourceNode;
  private volumeCheckInterval?: any;
  
  // Filler words to detect (common English filler words)
  private readonly commonFillerWords = [
    'um', 'uh', 'er', 'ah', 'like', 'you know', 'so', 'actually',
    'basically', 'literally', 'kind of', 'sort of', 'i mean',
    'well', 'okay', 'right', 'hmm', 'uhm', 'umm'
  ];

  // Tracking for pace analysis
  private wpmSamples: number[] = [];
  private pauseDurations: number[] = [];

  constructor() {
    // Auto-cleanup effect
    effect(() => {
      const state = this.recordingState();
      if (state === 'stopped' || state === 'idle') {
        this.stopSilenceDetection();
        this.stopVoiceEnergyAnalysis();
      }
    });
  }

  /**
   * Start recording analytics tracking
   */
  startRecording(mediaStream?: MediaStream): void {
    this.recordingState.set('active');
    this.microphoneStatus.set('recording');
    this.recordingDuration.set(0);
    this.wordCount.set(0);
    this.currentTranscript.set('');
    this.silenceCount.set(0);
    this.lastSilenceDuration.set(0);
    this.isLongPause.set(false);

    // Reset voice intelligence metrics
    this.resetVoiceIntelligenceMetrics();

    // Start timer
    this.startTimer();

    // Start silence detection
    this.startSilenceDetection();

    // Start voice energy analysis if media stream provided
    if (mediaStream) {
      this.startVoiceEnergyAnalysis(mediaStream);
    }

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
    this.stopVoiceEnergyAnalysis();
    
    // Finalize calculations
    this.finalizeVoiceIntelligenceMetrics();
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
    this.stopVoiceEnergyAnalysis();
    this.resetVoiceIntelligenceMetrics();
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
      
      // Sample WPM every second for average calculation
      const currentWPM = this.speakingSpeed();
      if (currentWPM > 0) {
        this.wpmSamples.push(currentWPM);
      }
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
        let fullTranscript = '';

        // Get all final results
        for (let i = 0; i < event.results.length; i++) {
          if (event.results[i].isFinal) {
            fullTranscript += event.results[i][0].transcript + ' ';
          }
        }

        // Add interim results from the last result
        if (event.results.length > 0) {
          const lastResult = event.results[event.results.length - 1];
          if (!lastResult.isFinal) {
            fullTranscript += lastResult[0].transcript;
          }
        }

        // Update transcript if we have new content
        if (fullTranscript.trim().length > 0) {
          this.updateTranscript(fullTranscript.trim());
        }
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

  // ──────────────────────────────────────────────────────────────
  // Voice Intelligence Engine - Module 2
  // ──────────────────────────────────────────────────────────────

  private resetVoiceIntelligenceMetrics(): void {
    this.averageWPM.set(0);
    this.paceRating.set('Good');
    this.fillerWordCount.set(0);
    this.mostUsedFillerWord.set('');
    this.fillerWordsMap.set(new Map());
    this.longestPause.set(0);
    this.pauseCount.set(0);
    this.averagePauseDuration.set(0);
    this.voiceEnergy.set(0);
    this.averageVolume.set(0);
    this.volumeSamples.set([]);
    this.fluencyScore.set(0);
    this.communicationScore.set(0);
    
    // Reset tracking arrays
    this.wpmSamples = [];
    this.pauseDurations = [];
  }

  private finalizeVoiceIntelligenceMetrics(): void {
    const transcript = this.currentTranscript().trim();
    if (!transcript) {
      this.averageWPM.set(0);
      this.paceRating.set('Good');
      this.fillerWordCount.set(0);
      this.mostUsedFillerWord.set('None');
      this.fillerWordsMap.set(new Map());
      this.longestPause.set(0);
      this.pauseCount.set(0);
      this.averagePauseDuration.set(0);
      this.voiceEnergy.set(0);
      this.averageVolume.set(0);
      this.volumeSamples.set([]);
      this.fluencyScore.set(0);
      this.communicationScore.set(0);
      this.wpmSamples = [];
      this.pauseDurations = [];
      return;
    }

    if (this.wpmSamples.length > 0) {
      const avgWPM = Math.round(
        this.wpmSamples.reduce((a, b) => a + b, 0) / this.wpmSamples.length
      );
      this.averageWPM.set(avgWPM);
      this.paceRating.set(this.calculatePaceRating(avgWPM));
    } else {
      this.averageWPM.set(0);
      this.paceRating.set('Good');
    }

    if (this.pauseDurations.length > 0) {
      const avgPause = this.pauseDurations.reduce((a, b) => a + b, 0) / this.pauseDurations.length;
      this.averagePauseDuration.set(Math.round(avgPause * 10) / 10);

      const maxPause = Math.max(...this.pauseDurations);
      this.longestPause.set(Math.round(maxPause * 10) / 10);
    } else {
      this.averagePauseDuration.set(0);
      this.longestPause.set(0);
    }

    this.detectFillerWords(transcript);
    this.calculateFluencyScore();
    this.calculateCommunicationScore();
  }

  private calculatePaceRating(wpm: number): PaceRating {
    if (wpm < 100) return 'Too Slow';
    if (wpm < 150) return 'Good';
    if (wpm < 180) return 'Excellent';
    return 'Too Fast';
  }

  private detectFillerWords(transcript: string): void {
    if (!transcript || transcript.trim().length === 0) return;

    const lowerTranscript = transcript.toLowerCase();
    const fillerMap = new Map<string, number>();
    let totalFillers = 0;

    // Count each filler word
    for (const filler of this.commonFillerWords) {
      // Use word boundaries for accurate matching
      const regex = new RegExp(`\\b${filler}\\b`, 'gi');
      const matches = lowerTranscript.match(regex);
      const count = matches ? matches.length : 0;
      
      if (count > 0) {
        fillerMap.set(filler, count);
        totalFillers += count;
      }
    }

    this.fillerWordsMap.set(fillerMap);
    this.fillerWordCount.set(totalFillers);

    // Find most used filler word
    if (fillerMap.size > 0) {
      const mostUsed = Array.from(fillerMap.entries())
        .sort((a, b) => b[1] - a[1])[0];
      this.mostUsedFillerWord.set(`${mostUsed[0]} (${mostUsed[1]}x)`);
    } else {
      this.mostUsedFillerWord.set('None');
    }
  }

  private calculateFluencyScore(): void {
    const words = this.wordCount();
    const recordingState = this.recordingState();

    if (words === 0 || recordingState !== 'active' && recordingState !== 'stopped') {
      this.fluencyScore.set(0);
      return;
    }

    // Fluency score based on multiple factors (0-100)
    let score = 100;

    const wpm = this.averageWPM();
    const fillers = this.fillerWordCount();
    const pauses = this.pauseCount();
    const avgPause = this.averagePauseDuration();

    // Penalty for pace issues (-20 points max)
    if (wpm < 80 || wpm > 200) {
      score -= 20;
    } else if (wpm < 100 || wpm > 180) {
      score -= 10;
    }

    // Penalty for filler words (-30 points max)
    const fillerRatio = fillers / words;
    if (fillerRatio > 0.15) {
      score -= 30;
    } else if (fillerRatio > 0.10) {
      score -= 20;
    } else if (fillerRatio > 0.05) {
      score -= 10;
    }

    // Penalty for excessive pauses (-20 points max)
    if (avgPause > 5) {
      score -= 20;
    } else if (avgPause > 3) {
      score -= 10;
    }

    // Penalty for too many pauses (-15 points max)
    const duration = this.recordingDuration();
    if (duration > 0) {
      const pausesPerMinute = (pauses / duration) * 60;
      if (pausesPerMinute > 4) {
        score -= 15;
      } else if (pausesPerMinute > 2) {
        score -= 8;
      }
    }

    this.fluencyScore.set(Math.max(0, Math.min(100, score)));
  }

  private calculateCommunicationScore(): void {
    const words = this.wordCount();
    const recordingState = this.recordingState();

    if (words === 0 || recordingState !== 'active' && recordingState !== 'stopped') {
      this.communicationScore.set(0);
      return;
    }

    // Communication score based on fluency, energy, and consistency (0-100)
    const fluency = this.fluencyScore();
    const energy = this.voiceEnergy();

    // Weight: 60% fluency, 40% energy
    const score = Math.round(fluency * 0.6 + energy * 0.4);

    this.communicationScore.set(Math.max(0, Math.min(100, score)));
  }

  private startVoiceEnergyAnalysis(mediaStream: MediaStream): void {
    try {
      // Create audio context
      this.audioContext = new (window.AudioContext || (window as any).webkitAudioContext)();
      this.analyser = this.audioContext.createAnalyser();
      this.analyser.fftSize = 256;
      
      // Connect microphone stream
      this.microphone = this.audioContext.createMediaStreamSource(mediaStream);
      this.microphone.connect(this.analyser);

      // Start monitoring volume
      this.monitorVoiceEnergy();
    } catch (error) {
      console.error('Failed to initialize voice energy analysis:', error);
    }
  }

  private monitorVoiceEnergy(): void {
    if (!this.analyser) return;

    const bufferLength = this.analyser.frequencyBinCount;
    const dataArray = new Uint8Array(bufferLength);

    const checkVolume = () => {
      if (this.recordingState() !== 'active' || !this.analyser) {
        return;
      }

      this.analyser.getByteFrequencyData(dataArray);

      // Calculate average volume
      let sum = 0;
      for (let i = 0; i < bufferLength; i++) {
        sum += dataArray[i];
      }
      const average = sum / bufferLength;
      
      // Normalize to 0-100 scale
      const normalizedVolume = Math.min(100, Math.round((average / 255) * 100));

      // Update volume samples
      const samples = [...this.volumeSamples(), normalizedVolume];
      if (samples.length > 100) {
        samples.shift(); // Keep only last 100 samples
      }
      this.volumeSamples.set(samples);

      // Calculate average volume
      const avgVolume = Math.round(
        samples.reduce((a, b) => a + b, 0) / samples.length
      );
      this.averageVolume.set(avgVolume);

      // Voice energy (0-100) based on volume consistency and level
      const energy = this.calculateVoiceEnergy(samples);
      this.voiceEnergy.set(energy);

      // Continue monitoring
      this.volumeCheckInterval = setTimeout(checkVolume, 100);
    };

    checkVolume();
  }

  private calculateVoiceEnergy(samples: number[]): number {
    if (samples.length < 10) return 0;

    const average = samples.reduce((a, b) => a + b, 0) / samples.length;
    
    // Calculate variance for consistency
    const variance = samples.reduce((sum, val) => sum + Math.pow(val - average, 2), 0) / samples.length;
    const stdDev = Math.sqrt(variance);
    
    // Energy score based on volume level and consistency
    // High volume + low variance = high energy
    const volumeScore = Math.min(100, average * 1.5);
    const consistencyScore = Math.max(0, 100 - stdDev * 2);
    
    return Math.round(volumeScore * 0.7 + consistencyScore * 0.3);
  }

  private stopVoiceEnergyAnalysis(): void {
    if (this.volumeCheckInterval) {
      clearTimeout(this.volumeCheckInterval);
      this.volumeCheckInterval = undefined;
    }

    if (this.microphone) {
      this.microphone.disconnect();
      this.microphone = undefined;
    }

    if (this.audioContext) {
      this.audioContext.close();
      this.audioContext = undefined;
    }

    this.analyser = undefined;
  }

  private onSilenceDetected(): void {
    this.silenceCount.update(c => c + 1);
    this.pauseCount.update(c => c + 1);
    this.isLongPause.set(true);
    
    // Calculate silence duration
    const timeSinceLastUpdate = Date.now() - this.lastTranscriptUpdate;
    const silenceDuration = timeSinceLastUpdate / 1000;
    this.lastSilenceDuration.set(Math.floor(silenceDuration));

    // Track pause duration for analysis
    this.pauseDurations.push(silenceDuration);

    // Continue monitoring for more silence
    this.resetSilenceTimer();
  }
}
