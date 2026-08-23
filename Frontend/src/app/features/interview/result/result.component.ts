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

  // Computed scores from backend result
  readonly overallScore = computed(() => {
    const session = this.session();
    return session?.result?.overallScore ?? 0;
  });

  readonly technicalScore = computed(() => {
    const session = this.session();
    return session?.result?.technicalScore ?? 0;
  });

  readonly communicationScore = computed(() => {
    const session = this.session();
    return session?.result?.communicationScore ?? 0;
  });

  readonly confidenceScore = computed(() => {
    const session = this.session();
    return session?.result?.confidenceScore ?? 0;
  });

  // AI Interview Coach (Module 5)
  readonly hasCoachingReport = computed(() => {
    const session = this.session();
    return !!(session?.result?.coachingSummary);
  });

  readonly coachingData = computed(() => {
    const session = this.session();
    if (!session?.result) return null;
    
    return {
      summary: session.result.coachingSummary || '',
      strengths: this.splitBySemicolon(session.result.coachingStrengths || ''),
      weaknesses: this.splitBySemicolon(session.result.coachingWeaknesses || ''),
      communicationImprovements: this.splitBySemicolon(session.result.communicationImprovements || ''),
      technicalImprovements: this.splitBySemicolon(session.result.technicalImprovements || ''),
      videoImprovements: this.splitBySemicolon(session.result.videoImprovements || ''),
      voiceImprovements: this.splitBySemicolon(session.result.voiceImprovements || ''),
      practiceRecommendations: this.splitBySemicolon(session.result.practiceRecommendations || ''),
      learningResources: this.splitBySemicolon(session.result.learningResources || ''),
      suggestedRole: session.result.suggestedRole || '',
      suggestedDifficulty: session.result.suggestedDifficulty || '',
      suggestedQuestionCount: session.result.suggestedQuestionCount || 0,
      motivationalMessage: session.result.motivationalMessage || ''
    };
  });

  // Company Intelligence (Module 6)
  readonly hasCompanyReadiness = computed(() => {
    const session = this.session();
    return (session?.result?.companyReadinessScore ?? 0) > 0;
  });

  readonly companyReadinessScore = computed(() => {
    const session = this.session();
    return session?.result?.companyReadinessScore ?? 0;
  });

  readonly technicalAlignment = computed(() => {
    const session = this.session();
    return session?.result?.technicalAlignment ?? 0;
  });

  readonly communicationAlignment = computed(() => {
    const session = this.session();
    return session?.result?.communicationAlignment ?? 0;
  });

  readonly cultureFit = computed(() => {
    const session = this.session();
    return session?.result?.cultureFit ?? 0;
  });

  readonly companySpecificFeedback = computed(() => {
    const session = this.session();
    return session?.result?.companySpecificFeedback ?? '';
  });

  // Resume Intelligence (Module 7)
  readonly hasResumeAnalysis = computed(() => {
    const session = this.session();
    return (session?.result?.resumeMatchScore ?? 0) > 0;
  });

  readonly resumeMatchScore = computed(() => {
    const session = this.session();
    return session?.result?.resumeMatchScore ?? 0;
  });

  readonly skillGapSummary = computed(() => {
    const session = this.session();
    return session?.result?.skillGapSummary ?? '';
  });

  readonly careerRecommendations = computed(() => {
    const session = this.session();
    return session?.result?.careerRecommendations ?? '';
  });

  private splitBySemicolon(text: string): string[] {
    return text.split(';').map(s => s.trim()).filter(s => s.length > 0);
  }

  readonly strengths = computed(() => {
    const session = this.session();
    if (!session?.result?.strengths) return [];

    // Split by semicolons and clean up
    return session.result.strengths
      .split(';')
      .map(s => s.trim())
      .filter(s => s.length > 0)
      .slice(0, 5);
  });

  readonly improvements = computed(() => {
    const session = this.session();
    if (!session?.result?.weaknesses) return [];

    // Split by semicolons and clean up
    return session.result.weaknesses
      .split(';')
      .map(s => s.trim())
      .filter(s => s.length > 0)
      .slice(0, 5);
  });

  readonly summary = computed(() => {
    const session = this.session();
    return session?.result?.summary ?? 'Complete the interview to see your summary.';
  });

  readonly recommendations = computed(() => {
    const session = this.session();
    if (!session?.result?.recommendations) return [];

    // Split by periods or semicolons and clean up
    return session.result.recommendations
      .split(/[.;]/)
      .map(s => s.trim())
      .filter(s => s.length > 0);
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
