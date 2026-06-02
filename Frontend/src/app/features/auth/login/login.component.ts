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

export type LoginMode = 'password' | 'audio' | 'video';
type RecordingState = 'idle' | 'recording' | 'stopped';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnDestroy {
  @ViewChild('videoEl') videoElRef!: ElementRef<HTMLVideoElement>;

  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly mode = signal<LoginMode>('password');
  readonly recordingState = signal<RecordingState>('idle');
  readonly loading = signal(false);
  readonly error = signal('');

  // Password form
  readonly passwordForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  // Email-only form (audio / video modes)
  readonly emailForm = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]]
  });

  stream: MediaStream | null = null;
  recordedBlob: Blob | null = null;
  private recorder: MediaRecorder | null = null;
  private chunks: BlobPart[] = [];

  // ── Mode switching ───────────────────────────────────────────
  setMode(m: LoginMode): void {
    this.stopStream();
    this.resetCapture();
    this.error.set('');
    this.mode.set(m);
  }

  // ── Password login ───────────────────────────────────────────
  submitPassword(): void {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }
    const { email, password } = this.passwordForm.getRawValue();
    this.callLogin(() => this.auth.loginPassword({ email, password }));
  }

  // ── Audio / Video capture ────────────────────────────────────
  async startCamera(): Promise<void> {
    this.error.set('');
    const constraints = this.mode() === 'audio'
      ? { audio: true, video: false }
      : { audio: true, video: true };
    try {
      this.stream = await navigator.mediaDevices.getUserMedia(constraints);
      if (this.mode() === 'video') {
        const video = this.videoElRef.nativeElement;
        video.srcObject = this.stream;
        video.muted = true;
        await video.play();
      }
      this.recordingState.set('idle');
    } catch {
      this.error.set('Could not access media devices. Please allow permissions and try again.');
    }
  }

  startRecording(): void {
    if (!this.stream) return;
    this.chunks = [];
    this.recordedBlob = null;

    const mimeType = this.mode() === 'audio'
      ? (MediaRecorder.isTypeSupported('audio/webm;codecs=opus') ? 'audio/webm;codecs=opus' : 'audio/webm')
      : (MediaRecorder.isTypeSupported('video/webm;codecs=vp9,opus') ? 'video/webm;codecs=vp9,opus' : 'video/webm');

    this.recorder = new MediaRecorder(this.stream, { mimeType });
    this.recorder.ondataavailable = (e) => {
      if (e.data.size > 0) this.chunks.push(e.data);
    };
    this.recorder.onstop = () => {
      this.recordedBlob = new Blob(this.chunks, { type: mimeType });
      this.stopStream();
    };
    this.recorder.start();
    this.recordingState.set('recording');
  }

  stopRecording(): void {
    this.recorder?.stop();
    this.recordingState.set('stopped');
  }

  retake(): void {
    this.resetCapture();
    this.startCamera();
  }

  submitAudio(): void {
    if (this.emailForm.invalid) { this.emailForm.markAllAsTouched(); return; }
    if (!this.recordedBlob) { this.error.set('No recording found. Please record first.'); return; }

    const fd = new FormData();
    fd.append('email', this.emailForm.getRawValue().email);
    fd.append('AudioFile', new File([this.recordedBlob], 'audio.webm', { type: this.recordedBlob.type }));
    this.callLogin(() => this.auth.loginAudio(fd));
  }

  submitVideo(): void {
    if (this.emailForm.invalid) { this.emailForm.markAllAsTouched(); return; }
    if (!this.recordedBlob) { this.error.set('No recording found. Please record first.'); return; }

    const fd = new FormData();
    fd.append('email', this.emailForm.getRawValue().email);
    fd.append('VideoFile', new File([this.recordedBlob], 'video.webm', { type: this.recordedBlob.type }));
    this.callLogin(() => this.auth.loginVideo(fd));
  }

  // ── Shared ───────────────────────────────────────────────────
  private callLogin(fn: () => ReturnType<AuthService['loginPassword']>): void {
    this.loading.set(true);
    this.error.set('');
    fn().subscribe({
      next: (response) => {
        this.auth.saveTokenSync(response.token);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.error.set(err?.error?.message ?? 'Login failed. Please try again.');
        this.loading.set(false);
      }
    });
  }

  private resetCapture(): void {
    this.chunks = [];
    this.recordedBlob = null;
    this.recordingState.set('idle');
  }

  private stopStream(): void {
    this.stream?.getTracks().forEach(t => t.stop());
    this.stream = null;
  }

  ngOnDestroy(): void {
    this.stopStream();
  }
}
