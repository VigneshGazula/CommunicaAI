import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MediaService } from '../../core/services/media.service';

@Component({
  selector: 'app-onboarding',
  standalone: true,
  imports: [],
  templateUrl: './onboarding.component.html',
  styleUrl: './onboarding.component.scss'
})
export class OnboardingComponent {
  private readonly media = inject(MediaService);
  private readonly router = inject(Router);

  readonly loading = signal(false);
  readonly error = signal('');

  audioFile: File | null = null;
  videoFile: File | null = null;

  onAudioChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.audioFile = input.files?.[0] ?? null;
  }

  onVideoChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.videoFile = input.files?.[0] ?? null;
  }

  submit(): void {
    if (!this.audioFile || !this.videoFile) {
      this.error.set('Please select both an audio file and a video file.');
      return;
    }

    const formData = new FormData();
    formData.append('AudioFile', this.audioFile);
    formData.append('VideoFile', this.videoFile);

    this.loading.set(true);
    this.error.set('');

    this.media.submitOnboarding(formData).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: (err) => {
        this.error.set(err?.error?.message ?? 'Upload failed. Please try again.');
        this.loading.set(false);
      }
    });
  }
}
