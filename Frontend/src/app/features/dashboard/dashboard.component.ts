import { Component, inject, signal, afterNextRender } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { InterviewHistoryService } from '../../core/services/interview-history.service';
import { UserProfile } from '../../core/models/auth.models';
import { InterviewStats, InterviewResult } from '../../core/models/interview.models';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  private readonly auth = inject(AuthService);
  private readonly history = inject(InterviewHistoryService);
  private readonly router = inject(Router);

  readonly user = signal<UserProfile | null>(null);
  readonly stats = signal<InterviewStats | null>(null);
  readonly recentSessions = signal<InterviewResult[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  constructor() {
    afterNextRender(() => {
      forkJoin({
        user: this.auth.me(),
        stats: this.history.getStats(),
        sessions: this.history.listSessions()
      }).subscribe({
        next: ({ user, stats, sessions }) => {
          this.user.set(user);
          this.stats.set(stats);
          this.recentSessions.set(sessions.slice(0, 3)); // Show last 3
          this.loading.set(false);
        },
        error: () => {
          this.auth.logout();
          this.router.navigate(['/login']);
        }
      });
    });
  }

  startInterview(): void {
    this.router.navigate(['/interview/setup']);
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
