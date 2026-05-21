import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { MediaService } from '../../core/services/media.service';
import { UserProfile } from '../../core/models/auth.models';
import { MediaItem } from '../../core/models/media.models';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly media = inject(MediaService);
  private readonly router = inject(Router);

  readonly user = signal<UserProfile | null>(null);
  readonly mediaItems = signal<MediaItem[]>([]);
  readonly loadingUser = signal(true);
  readonly loadingMedia = signal(true);
  readonly error = signal('');

  ngOnInit(): void {
    this.auth.me().subscribe({
      next: (profile) => {
        this.user.set(profile);
        this.loadingUser.set(false);
      },
      error: () => this.logout()
    });

    this.media.getMyMedia().subscribe({
      next: (items) => {
        this.mediaItems.set(items);
        this.loadingMedia.set(false);
      },
      error: (err) => {
        this.error.set(err?.error?.message ?? 'Failed to load media.');
        this.loadingMedia.set(false);
      }
    });
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
