import {
  Component,
  ElementRef,
  inject,
  OnDestroy,
  signal,
  ViewChild
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

type Step = 'form' | 'video' | 'audio' | 'review';

const FUNNY_QUOTES = [
  'Why do programmers prefer dark mode? Because light attracts bugs!',
  'I told my computer I needed a break. Now it won\'t stop sending me Kit-Kat ads.',
  'A SQL query walks into a bar, walks up to two tables and asks… can I join you?',
  'There are 10 types of people: those who understand binary and those who don\'t.',
  'My code never has bugs. It just develops random features.',
  'I would love to change the world, but they won\'t give me the source code.',
  'Debugging: being the detective in a crime movie where you are also the murderer.',
  'It works on my machine — so we\'re shipping my machine.',
];

const VIDEO_LIMIT_MS = 5000;
const AUDIO_LIMIT_MS = 5000;

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnDestroy {
  @ViewChild('videoEl') videoElRef!: ElementRef<HTMLVideoElement>;

  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly step = signal<Step>('form');
  readonly loading = signal(false);
  readonly error = signal('');

  // video step state
  readonly videoReady = signal(false);       // camera is live
  readonly videoRecording = signal(false);
  readonly videoTimeLeft = signal(5);

  // audio step state
  readonly audioRecording = signal(false);
  readonly audioTimeLeft = signal(5);
  readonly funnyQuote = signal('');

  readonly form = this.fb.nonNullable.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  stream: MediaStream | null = null;
  videoBlob: Blob | null = null;
  audioBlob: Blob | null = null;

  private recorder: MediaRecorder | null = null;
  private chunks: BlobPart[] = [];
  private timerInterval: ReturnType<typeof setInterval> | null = null;
  private autoStopTimeout: ReturnType<typeof setTimeout> | null = null;

  // ─── Step 1 → 2 ──────────────────────────────────────────────
  proceedToVideo(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.error.set('');
    this.step.set('video');
  }

  backToForm(): void {
    this.stopStream();
    this.clearTimers();
    this.videoReady.set(false);
    this.videoRecording.set(false);
    this.videoTimeLeft.set(5);
    this.videoBlob = null;
    this.step.set('form');
  }

  // ─── Video capture ────────────────────────────────────────────
  async startVideoCamera(): Promise<void> {
    this.error.set('');
    try {
      this.stream = await navigator.mediaDevices.getUserMedia({
        video: { facingMode: 'user', width: { ideal: 640 }, height: { ideal: 480 } },
        audio: false   // video-only stream for face capture
      });
      const el = this.videoElRef.nativeElement;
      el.srcObject = this.stream;
      el.muted = true;
      await el.play();
      this.videoReady.set(true);
    } catch {
      this.error.set('Could not access camera. Please allow camera permission and try again.');
    }
  }

  startVideoRecording(): void {
    if (!this.stream) return;
    this.chunks = [];
    this.videoBlob = null;
    this.videoTimeLeft.set(5);

    const mimeType = MediaRecorder.isTypeSupported('video/webm;codecs=vp9')
      ? 'video/webm;codecs=vp9'
      : 'video/webm';

    this.recorder = new MediaRecorder(this.stream, { mimeType });
    this.recorder.ondataavailable = (e) => { if (e.data.size > 0) this.chunks.push(e.data); };
    this.recorder.onstop = () => {
      this.videoBlob = new Blob(this.chunks, { type: mimeType });
      this.clearTimers();
      this.stopStream();
      this.videoRecording.set(false);
      this.proceedToAudio();
    };
    this.recorder.start();
    this.videoRecording.set(true);

    // countdown
    this.timerInterval = setInterval(() => {
      const t = this.videoTimeLeft() - 1;
      this.videoTimeLeft.set(t);
      if (t <= 0) this.stopVideoRecording();
    }, 1000);

    // hard stop at 5s
    this.autoStopTimeout = setTimeout(() => this.stopVideoRecording(), VIDEO_LIMIT_MS);
  }

  stopVideoRecording(): void {
    if (this.recorder?.state === 'recording') {
      this.recorder.stop();
    }
    this.clearTimers();
  }

  retakeVideo(): void {
    this.stopStream();
    this.clearTimers();
    this.videoBlob = null;
    this.videoReady.set(false);
    this.videoRecording.set(false);
    this.videoTimeLeft.set(5);
  }

  // ─── Step 2 → 3 ──────────────────────────────────────────────
  private proceedToAudio(): void {
    this.funnyQuote.set(FUNNY_QUOTES[Math.floor(Math.random() * FUNNY_QUOTES.length)]);
    this.audioTimeLeft.set(5);
    this.step.set('audio');
  }

  backToVideo(): void {
    this.stopStream();
    this.clearTimers();
    this.videoBlob = null;
    this.audioBlob = null;
    this.videoReady.set(false);
    this.videoRecording.set(false);
    this.videoTimeLeft.set(5);
    this.audioRecording.set(false);
    this.audioTimeLeft.set(5);
    this.step.set('video');
  }

  // ─── Audio capture ────────────────────────────────────────────
  async startAudioRecording(): Promise<void> {
    this.error.set('');
    try {
      this.stream = await navigator.mediaDevices.getUserMedia({ audio: true, video: false });
    } catch {
      this.error.set('Could not access microphone. Please allow microphone permission and try again.');
      return;
    }

    this.chunks = [];
    this.audioBlob = null;
    this.audioTimeLeft.set(5);

    const mimeType = MediaRecorder.isTypeSupported('audio/webm;codecs=opus')
      ? 'audio/webm;codecs=opus'
      : 'audio/webm';

    this.recorder = new MediaRecorder(this.stream, { mimeType });
    this.recorder.ondataavailable = (e) => { if (e.data.size > 0) this.chunks.push(e.data); };
    this.recorder.onstop = () => {
      this.audioBlob = new Blob(this.chunks, { type: mimeType });
      this.clearTimers();
      this.stopStream();
      this.audioRecording.set(false);
      this.step.set('review');
    };
    this.recorder.start();
    this.audioRecording.set(true);

    // countdown
    this.timerInterval = setInterval(() => {
      const t = this.audioTimeLeft() - 1;
      this.audioTimeLeft.set(t);
      if (t <= 0) this.stopAudioRecording();
    }, 1000);

    // hard stop at 5s
    this.autoStopTimeout = setTimeout(() => this.stopAudioRecording(), AUDIO_LIMIT_MS);
  }

  stopAudioRecording(): void {
    if (this.recorder?.state === 'recording') {
      this.recorder.stop();
    }
    this.clearTimers();
  }

  // ─── Step 3 → back ───────────────────────────────────────────
  backToAudio(): void {
    this.audioBlob = null;
    this.audioRecording.set(false);
    this.audioTimeLeft.set(5);
    this.step.set('audio');
  }

  // ─── Submit ───────────────────────────────────────────────────
  submit(): void {
    if (!this.videoBlob || !this.audioBlob) {
      this.error.set('Missing recording. Please go back and try again.');
      return;
    }

    const { fullName, email, password } = this.form.getRawValue();
    const fd = new FormData();
    fd.append('fullName', fullName);
    fd.append('email', email);
    fd.append('password', password);
    fd.append('VideoFile', new File([this.videoBlob], 'video.webm', { type: this.videoBlob.type }));
    fd.append('AudioFile', new File([this.audioBlob], 'audio.webm', { type: this.audioBlob.type }));

    this.loading.set(true);
    this.error.set('');

    this.auth.register(fd).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        this.error.set(err?.error?.message ?? 'Registration failed. Please try again.');
        this.loading.set(false);
      }
    });
  }

  // ─── Helpers ─────────────────────────────────────────────────
  private stopStream(): void {
    this.stream?.getTracks().forEach(t => t.stop());
    this.stream = null;
  }

  private clearTimers(): void {
    if (this.timerInterval) { clearInterval(this.timerInterval); this.timerInterval = null; }
    if (this.autoStopTimeout) { clearTimeout(this.autoStopTimeout); this.autoStopTimeout = null; }
  }

  ngOnDestroy(): void {
    this.stopStream();
    this.clearTimers();
  }
}
