import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe, TitleCasePipe, NgClass } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { InterviewService } from '../../../core/services/interview.service';
import { InterviewHistoryResponse } from '../../../core/models/interview.models';

interface HistorySession {
  sessionId: string;
  role: string;
  difficulty: string;
  status: string;
  startedAt: Date;
  completedAt: Date | null;
  completionPercentage: number;
  interviewType: string; // Module 9
}

@Component({
  selector: 'app-interview-history',
  standalone: true,
  imports: [RouterLink, DatePipe, TitleCasePipe, NgClass],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent implements OnInit {
  private readonly interviewService = inject(InterviewService);
  private readonly router = inject(Router);

  readonly sessions = signal<HistorySession[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');

  ngOnInit(): void {
    this.loadHistory();
  }

  private loadHistory(): void {
    this.interviewService.getUserHistory().subscribe({
      next: (history) => {
        // Transform backend response to display format
        const sessions: HistorySession[] = history
          .map(h => ({
            sessionId: h.sessionId,
            role: h.role,
            difficulty: h.difficulty,
            status: h.status,
            startedAt: new Date(h.startedAt),
            completedAt: h.completedAt ? new Date(h.completedAt) : null,
            completionPercentage: h.completionPercentage || 0,
            interviewType: h.interviewType || 'Technical' // Module 9
          }))
          // Sort by most recent first
          .sort((a, b) => {
            const dateA = a.completedAt || a.startedAt;
            const dateB = b.completedAt || b.startedAt;
            return dateB.getTime() - dateA.getTime();
          });

        this.sessions.set(sessions);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading interview history:', err);
        this.error.set('Failed to load interview history');
        this.loading.set(false);

        // If unauthorized, redirect to login
        if (err.status === 401) {
          this.router.navigate(['/login']);
        }
      }
    });
  }

  getScoreBadgeClass(score: number): string {
    if (score >= 80) return 'badge-success';
    if (score >= 60) return 'badge-warning';
    return 'badge-danger';
  }

  getStatusLabel(status: string): string {
    switch (status.toLowerCase()) {
      case 'completed': return 'Completed';
      case 'in-progress': return 'In Progress';
      case 'inprogress': return 'In Progress';
      default: return status;
    }
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'completed': return 'status-completed';
      case 'in-progress': return 'status-progress';
      case 'inprogress': return 'status-progress';
      default: return 'status-draft';
    }
  }
}
