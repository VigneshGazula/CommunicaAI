import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DatePipe, TitleCasePipe } from '@angular/common';
import { InterviewHistoryService } from '../../../core/services/interview-history.service';
import { InterviewResult } from '../../../core/models/interview.models';

@Component({
  selector: 'app-interview-result',
  standalone: true,
  imports: [RouterLink, DatePipe, TitleCasePipe],
  templateUrl: './result.component.html',
  styleUrl: './result.component.scss'
})
export class ResultComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly historyService = inject(InterviewHistoryService);

  readonly result = signal<InterviewResult | null>(null);
  readonly loading = signal(true);
  readonly copySuccess = signal(false);

  ngOnInit(): void {
    const sessionId = this.route.snapshot.paramMap.get('sessionId') || '';
    if (!sessionId) {
      this.router.navigate(['/dashboard']);
      return;
    }

    this.historyService.getSessionById(sessionId).subscribe({
      next: (result) => {
        if (result) {
          this.result.set(result);
        } else {
          this.router.navigate(['/dashboard']);
        }
        this.loading.set(false);
      },
      error: () => {
        this.router.navigate(['/dashboard']);
      }
    });
  }

  copyTranscript(): void {
    const transcript = this.result()?.transcript;
    if (!transcript) return;

    navigator.clipboard.writeText(transcript).then(() => {
      this.copySuccess.set(true);
      setTimeout(() => this.copySuccess.set(false), 2000);
    });
  }

  getScoreColor(score: number): string {
    if (score >= 80) return '#10b981';
    if (score >= 60) return '#f59e0b';
    return '#ef4444';
  }
}
