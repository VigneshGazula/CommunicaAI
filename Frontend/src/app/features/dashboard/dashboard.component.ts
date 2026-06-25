import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { DatePipe, TitleCasePipe } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';
import { InterviewService } from '../../core/services/interview.service';
import { UserProfile } from '../../core/models/auth.models';
import { InterviewHistoryResponse } from '../../core/models/interview.models';
import { forkJoin } from 'rxjs';

interface DashboardSession {
  sessionId: string;
  role: string;
  difficulty: string;
  status: string;
  startedAt: Date;
  completedAt: Date | null;
  completionPercentage: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink, DatePipe, TitleCasePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly interviewService = inject(InterviewService);
  private readonly router = inject(Router);

  readonly user = signal<UserProfile | null>(null);
  readonly history = signal<InterviewHistoryResponse[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  // Computed values from history
  readonly totalInterviews = computed(() => this.history().length);
  
  readonly completedInterviews = computed(() => 
    this.history().filter(h => h.status.toLowerCase() === 'completed').length
  );

  readonly averageScore = computed(() => {
    const completed = this.history().filter(h => 
      h.status.toLowerCase() === 'completed' && h.completionPercentage !== null
    );
    
    if (completed.length === 0) return 0;
    
    const sum = completed.reduce((acc, h) => acc + (h.completionPercentage || 0), 0);
    return Math.round(sum / completed.length);
  });

  readonly recentSessions = computed(() => {
    return this.history()
      .filter(h => h.completedAt !== null)
      .sort((a, b) => new Date(b.completedAt!).getTime() - new Date(a.completedAt!).getTime())
      .slice(0, 3)
      .map(h => ({
        sessionId: h.sessionId,
        role: h.role,
        difficulty: h.difficulty,
        status: h.status,
        startedAt: new Date(h.startedAt),
        completedAt: h.completedAt ? new Date(h.completedAt) : null,
        completionPercentage: h.completionPercentage || 0
      }));
  });

  readonly currentStreak = computed(() => {
    const completed = this.history()
      .filter(h => h.status.toLowerCase() === 'completed' && h.completedAt !== null)
      .sort((a, b) => new Date(b.completedAt!).getTime() - new Date(a.completedAt!).getTime());

    if (completed.length === 0) return 0;

    let streak = 0;
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    for (const session of completed) {
      const sessionDate = new Date(session.completedAt!);
      sessionDate.setHours(0, 0, 0, 0);
      
      const daysDiff = Math.floor((today.getTime() - sessionDate.getTime()) / (1000 * 60 * 60 * 24));
      
      if (daysDiff <= streak + 1) {
        streak++;
      } else {
        break;
      }
    }

    return streak;
  });

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    forkJoin({
      user: this.auth.me(),
      history: this.interviewService.getUserHistory()
    }).subscribe({
      next: ({ user, history }) => {
        this.user.set(user);
        this.history.set(history);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading dashboard data:', err);
        this.error.set('Failed to load dashboard data');
        this.loading.set(false);
        
        // If unauthorized, redirect to login
        if (err.status === 401) {
          this.auth.logout();
          this.router.navigate(['/login']);
        }
      }
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
