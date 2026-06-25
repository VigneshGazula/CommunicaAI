import { Component, inject, signal, OnInit, computed } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DatePipe, TitleCasePipe } from '@angular/common';
import { InterviewService } from '../../../core/services/interview.service';
import { InterviewSession } from '../../../core/models/interview.models';

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
  private readonly interviewService = inject(InterviewService);

  readonly session = signal<InterviewSession | null>(null);
  readonly loading = signal(true);
  readonly copySuccess = signal(false);

  // Computed scores from answer evaluations
  readonly overallScore = computed(() => {
    const session = this.session();
    if (!session) return 0;

    const evaluations = session.answers
      .map(a => a.evaluation)
      .filter(e => e !== undefined);

    if (evaluations.length === 0) return 0;

    const avgScore = evaluations.reduce((sum, e) => sum + e!.overallScore, 0) / evaluations.length;
    return Math.round(avgScore);
  });

  readonly technicalScore = computed(() => {
    const session = this.session();
    if (!session) return 0;

    const evaluations = session.answers
      .map(a => a.evaluation)
      .filter(e => e !== undefined);

    if (evaluations.length === 0) return 0;

    const avgScore = evaluations.reduce((sum, e) => sum + e!.technicalScore, 0) / evaluations.length;
    return Math.round(avgScore);
  });

  readonly communicationScore = computed(() => {
    const session = this.session();
    if (!session) return 0;

    const evaluations = session.answers
      .map(a => a.evaluation)
      .filter(e => e !== undefined);

    if (evaluations.length === 0) return 0;

    const avgScore = evaluations.reduce((sum, e) => sum + e!.clarityScore, 0) / evaluations.length;
    return Math.round(avgScore);
  });

  readonly confidenceScore = computed(() => {
    const session = this.session();
    if (!session) return 0;

    const evaluations = session.answers
      .map(a => a.evaluation)
      .filter(e => e !== undefined);

    if (evaluations.length === 0) return 0;

    const avgScore = evaluations.reduce((sum, e) => sum + e!.completenessScore, 0) / evaluations.length;
    return Math.round(avgScore);
  });

  readonly strengths = computed(() => {
    const session = this.session();
    if (!session) return [];

    // Collect all unique strengths from evaluations
    const allStrengths = new Set<string>();
    
    session.answers.forEach(answer => {
      if (answer.evaluation?.strengths) {
        // Split by common delimiters and clean up
        const strengthItems = answer.evaluation.strengths
          .split(/[,;.]/)
          .map(s => s.trim())
          .filter(s => s.length > 0);
        
        strengthItems.forEach(s => allStrengths.add(s));
      }
    });

    return Array.from(allStrengths).slice(0, 5);
  });

  readonly improvements = computed(() => {
    const session = this.session();
    if (!session) return [];

    // Collect all unique improvements from evaluations
    const allImprovements = new Set<string>();
    
    session.answers.forEach(answer => {
      if (answer.evaluation?.improvements) {
        // Split by common delimiters and clean up
        const improvementItems = answer.evaluation.improvements
          .split(/[,;.]/)
          .map(s => s.trim())
          .filter(s => s.length > 0);
        
        improvementItems.forEach(s => allImprovements.add(s));
      }
    });

    return Array.from(allImprovements).slice(0, 5);
  });

  readonly summary = computed(() => {
    const session = this.session();
    if (!session) return '';

    // Combine all feedback into a summary
    const allFeedback = session.answers
      .map(a => a.evaluation?.feedback)
      .filter(f => f && f.length > 0)
      .join(' ');

    // Return first 500 characters as summary
    return allFeedback.slice(0, 500) + (allFeedback.length > 500 ? '...' : '');
  });

  readonly recommendations = computed(() => {
    const score = this.overallScore();
    const technicalScore = this.technicalScore();
    const communicationScore = this.communicationScore();
    const confidenceScore = this.confidenceScore();

    const recs: string[] = [];

    if (technicalScore < 70) {
      recs.push('Review fundamental concepts and practice technical problem-solving');
    }
    if (communicationScore < 70) {
      recs.push('Work on articulating your thoughts more clearly and concisely');
    }
    if (confidenceScore < 70) {
      recs.push('Provide more complete answers with specific examples and details');
    }
    if (score >= 80) {
      recs.push('Excellent performance! Keep practicing to maintain your skills');
    }
    if (score < 60) {
      recs.push('Consider taking additional courses or tutorials in weak areas');
    }

    return recs.length > 0 ? recs : ['Continue practicing interview skills regularly'];
  });

  ngOnInit(): void {
    const sessionId = this.route.snapshot.paramMap.get('sessionId') || '';
    if (!sessionId) {
      this.router.navigate(['/dashboard']);
      return;
    }

    // Load complete session details from backend
    this.interviewService.loadSessionDetails(sessionId).subscribe({
      next: (session) => {
        this.session.set(session);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading interview results:', err);
        this.router.navigate(['/dashboard']);
      }
    });
  }

  copyTranscript(): void {
    const session = this.session();
    if (!session) return;

    const transcript = session.questions
      .map((q, i) => {
        const answer = session.answers.find(a => a.questionId === q.id);
        const answerText = answer?.text || '(No answer provided)';
        return `Q${i + 1}: ${q.text}\n\nA${i + 1}: ${answerText}\n\n`;
      })
      .join('---\n\n');

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
