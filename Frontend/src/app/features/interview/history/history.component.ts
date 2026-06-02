import { Component, inject, signal, OnInit } from '@angular/core';
import { DatePipe, TitleCasePipe, NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { InterviewHistoryService } from '../../../core/services/interview-history.service';
import { InterviewResult } from '../../../core/models/interview.models';

@Component({
  selector: 'app-interview-history',
  standalone: true,
  imports: [RouterLink, DatePipe, TitleCasePipe, NgClass],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent implements OnInit {
  private readonly historyService = inject(InterviewHistoryService);

  readonly sessions = signal<InterviewResult[]>([]);
  readonly loading = signal(true);

  ngOnInit(): void {
    this.historyService.listSessions().subscribe({
      next: (sessions) => {
        this.sessions.set(sessions);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  getScoreBadgeClass(score: number): string {
    if (score >= 80) return 'badge-success';
    if (score >= 60) return 'badge-warning';
    return 'badge-danger';
  }
}
