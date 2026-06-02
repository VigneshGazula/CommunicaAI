import { Component, inject, signal, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { TitleCasePipe } from '@angular/common';
import { InterviewService } from '../../../core/services/interview.service';
import { InterviewHistoryService } from '../../../core/services/interview-history.service';
import { InterviewSession, InterviewQuestion } from '../../../core/models/interview.models';

@Component({
  selector: 'app-live-interview',
  standalone: true,
  imports: [ReactiveFormsModule, TitleCasePipe],
  templateUrl: './live.component.html',
  styleUrl: './live.component.scss'
})
export class LiveComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly interviewService = inject(InterviewService);
  private readonly historyService = inject(InterviewHistoryService);

  readonly session = signal<InterviewSession | null>(null);
  readonly currentQuestion = signal<InterviewQuestion | null>(null);
  readonly timeRemaining = signal(0);
  readonly loading = signal(false);
  readonly error = signal('');

  readonly answerForm = this.fb.nonNullable.group({
    answer: ['']
  });

  private sessionId = '';
  private timerInterval?: any;

  ngOnInit(): void {
    this.sessionId = this.route.snapshot.paramMap.get('sessionId') || '';
    if (!this.sessionId) {
      this.router.navigate(['/dashboard']);
      return;
    }

    const session = this.interviewService.getCurrentSession();
    if (!session || session.id !== this.sessionId) {
      this.router.navigate(['/dashboard']);
      return;
    }

    this.session.set(session);
    this.updateCurrentQuestion();
    this.startTimer();
    this.loadExistingAnswer();
  }

  ngOnDestroy(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
  }

  private startTimer(): void {
    const session = this.session();
    if (!session) return;

    const totalSeconds = session.setup.duration * 60;
    this.timeRemaining.set(totalSeconds);

    this.timerInterval = setInterval(() => {
      const remaining = this.timeRemaining();
      if (remaining > 0) {
        this.timeRemaining.set(remaining - 1);
      } else {
        clearInterval(this.timerInterval);
        this.finishInterview();
      }
    }, 1000);
  }

  private updateCurrentQuestion(): void {
    const session = this.session();
    if (!session) return;

    const question = session.questions[session.currentQuestionIndex];
    this.currentQuestion.set(question || null);
  }

  private loadExistingAnswer(): void {
    const session = this.session();
    const question = this.currentQuestion();
    if (!session || !question) return;

    const existingAnswer = session.answers.find(a => a.questionId === question.id);
    if (existingAnswer) {
      this.answerForm.patchValue({ answer: existingAnswer.text });
    } else {
      this.answerForm.patchValue({ answer: '' });
    }
  }

  saveCurrentAnswer(): void {
    const session = this.session();
    const question = this.currentQuestion();
    if (!session || !question) return;

    const answerText = this.answerForm.value.answer?.trim() || '';
    if (!answerText) return;

    this.interviewService.saveAnswer(session.id, {
      questionId: question.id,
      text: answerText,
      timestamp: new Date()
    }).subscribe();
  }

  nextQuestion(): void {
    this.saveCurrentAnswer();

    const session = this.session();
    if (!session) return;

    if (session.currentQuestionIndex < session.questions.length - 1) {
      const newIndex = session.currentQuestionIndex + 1;
      this.interviewService.updateQuestionIndex(session.id, newIndex).subscribe(() => {
        const updatedSession = this.interviewService.getCurrentSession();
        if (updatedSession) {
          this.session.set(updatedSession);
          this.updateCurrentQuestion();
          this.loadExistingAnswer();
        }
      });
    }
  }

  previousQuestion(): void {
    this.saveCurrentAnswer();

    const session = this.session();
    if (!session) return;

    if (session.currentQuestionIndex > 0) {
      const newIndex = session.currentQuestionIndex - 1;
      this.interviewService.updateQuestionIndex(session.id, newIndex).subscribe(() => {
        const updatedSession = this.interviewService.getCurrentSession();
        if (updatedSession) {
          this.session.set(updatedSession);
          this.updateCurrentQuestion();
          this.loadExistingAnswer();
        }
      });
    }
  }

  finishInterview(): void {
    this.saveCurrentAnswer();
    this.loading.set(true);

    const session = this.session();
    if (!session) return;

    this.interviewService.finishSession(session.id).subscribe({
      next: (result) => {
        this.historyService.saveSession(result).subscribe(() => {
          this.router.navigate(['/interview/result', session.id]);
        });
      },
      error: () => {
        this.error.set('Failed to finish interview. Please try again.');
        this.loading.set(false);
      }
    });
  }

  get formattedTime(): string {
    const remaining = this.timeRemaining();
    const minutes = Math.floor(remaining / 60);
    const seconds = remaining % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }

  get isLastQuestion(): boolean {
    const session = this.session();
    if (!session) return false;
    return session.currentQuestionIndex === session.questions.length - 1;
  }

  get isFirstQuestion(): boolean {
    const session = this.session();
    if (!session) return false;
    return session.currentQuestionIndex === 0;
  }
}
