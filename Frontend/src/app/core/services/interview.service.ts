import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import {
  InterviewSetup,
  InterviewSession,
  InterviewQuestion,
  InterviewAnswer,
  InterviewResult
} from '../models/interview.models';

@Injectable({ providedIn: 'root' })
export class InterviewService {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly STORAGE_KEY = 'communica_current_session';

  // Mock question bank
  private readonly questionBank: Record<string, string[]> = {
    'Software Engineer': [
      'Tell me about your experience with full-stack development.',
      'How do you handle code reviews and feedback?',
      'Describe a challenging bug you recently solved.',
      'What is your approach to writing maintainable code?',
      'How do you stay updated with new technologies?'
    ],
    'Product Manager': [
      'How do you prioritize features in a product roadmap?',
      'Describe a time you had to make a difficult trade-off decision.',
      'How do you gather and validate user requirements?',
      'What metrics do you use to measure product success?',
      'How do you handle stakeholder disagreements?'
    ],
    'Data Scientist': [
      'Explain your approach to exploratory data analysis.',
      'How do you handle imbalanced datasets?',
      'Describe a machine learning project you completed.',
      'What is your process for feature engineering?',
      'How do you communicate technical findings to non-technical stakeholders?'
    ],
    'Marketing Manager': [
      'How do you measure the ROI of marketing campaigns?',
      'Describe your experience with digital marketing channels.',
      'How do you develop a go-to-market strategy?',
      'What tools do you use for market research?',
      'How do you balance brand awareness and lead generation?'
    ]
  };

  createSession(setup: InterviewSetup): Observable<InterviewSession> {
    const session: InterviewSession = {
      id: this.generateId(),
      setup,
      questions: this.generateQuestions(setup),
      answers: [],
      status: 'in-progress',
      createdAt: new Date(),
      currentQuestionIndex: 0
    };

    this.saveCurrentSession(session);
    return of(session).pipe(delay(300));
  }

  getCurrentSession(): InterviewSession | null {
    if (!isPlatformBrowser(this.platformId)) return null;
    const stored = localStorage.getItem(this.STORAGE_KEY);
    if (!stored) return null;
    
    const session = JSON.parse(stored);
    session.createdAt = new Date(session.createdAt);
    session.answers = session.answers.map((a: any) => ({
      ...a,
      timestamp: new Date(a.timestamp)
    }));
    return session;
  }

  saveAnswer(sessionId: string, answer: InterviewAnswer): Observable<void> {
    const session = this.getCurrentSession();
    if (!session || session.id !== sessionId) {
      throw new Error('Session not found');
    }

    // Remove existing answer for this question if any
    session.answers = session.answers.filter(a => a.questionId !== answer.questionId);
    session.answers.push(answer);
    
    this.saveCurrentSession(session);
    return of(void 0).pipe(delay(100));
  }

  saveTranscript(sessionId: string, questionId: string, transcript: string): Observable<void> {
    const session = this.getCurrentSession();
    if (!session || session.id !== sessionId) {
      throw new Error('Session not found');
    }

    // Find or create answer for this question
    let answer = session.answers.find(a => a.questionId === questionId);
    if (answer) {
      answer.text = transcript;
      answer.timestamp = new Date();
    } else {
      answer = {
        questionId,
        text: transcript,
        timestamp: new Date()
      };
      session.answers.push(answer);
    }
    
    this.saveCurrentSession(session);
    return of(void 0).pipe(delay(100));
  }

  updateQuestionIndex(sessionId: string, index: number): Observable<void> {
    const session = this.getCurrentSession();
    if (!session || session.id !== sessionId) {
      throw new Error('Session not found');
    }

    session.currentQuestionIndex = index;
    this.saveCurrentSession(session);
    return of(void 0);
  }

  finishSession(sessionId: string): Observable<InterviewResult> {
    const session = this.getCurrentSession();
    if (!session || session.id !== sessionId) {
      throw new Error('Session not found');
    }

    session.status = 'completed';
    session.completedAt = new Date();
    this.saveCurrentSession(session);

    const result = this.computeResult(session);
    this.clearCurrentSession();
    
    return of(result).pipe(delay(500));
  }

  private generateQuestions(setup: InterviewSetup): InterviewQuestion[] {
    const baseQuestions = this.questionBank[setup.role] || this.questionBank['Software Engineer'];
    const questions: InterviewQuestion[] = [];
    
    for (let i = 0; i < setup.questionCount; i++) {
      const questionText = baseQuestions[i % baseQuestions.length];
      questions.push({
        id: this.generateId(),
        text: questionText,
        order: i + 1
      });
    }
    
    return questions;
  }

  private computeResult(session: InterviewSession): InterviewResult {
    // Mock scoring logic
    const answerCount = session.answers.length;
    const totalQuestions = session.questions.length;
    const completionRate = totalQuestions > 0 ? answerCount / totalQuestions : 0;

    const avgAnswerLength = session.answers.length > 0
      ? session.answers.reduce((sum, a) => sum + a.text.length, 0) / session.answers.length
      : 0;

    const overallScore = Math.round(
      (completionRate * 0.5 + Math.min(avgAnswerLength / 500, 1) * 0.5) * 100
    );

    const communicationScore = Math.round(Math.min(avgAnswerLength / 400, 1) * 100);
    const confidenceScore = Math.round((completionRate * 0.7 + Math.random() * 0.3) * 100);

    const transcript = session.questions
      .map((q, i) => {
        const answer = session.answers.find(a => a.questionId === q.id);
        return `Q${i + 1}: ${q.text}\n\nA${i + 1}: ${answer?.text || '(No answer provided)'}\n\n`;
      })
      .join('---\n\n');

    return {
      sessionId: session.id,
      overallScore,
      communicationScore,
      confidenceScore,
      strengths: this.generateStrengths(overallScore),
      improvements: this.generateImprovements(overallScore),
      transcript,
      setup: session.setup,
      completedAt: session.completedAt || new Date()
    };
  }

  private generateStrengths(score: number): string[] {
    const allStrengths = [
      'Clear and concise communication',
      'Strong technical knowledge',
      'Good problem-solving approach',
      'Confident delivery',
      'Well-structured responses'
    ];

    const count = score >= 80 ? 4 : score >= 60 ? 3 : 2;
    return allStrengths.slice(0, count);
  }

  private generateImprovements(score: number): string[] {
    const allImprovements = [
      'Provide more specific examples',
      'Expand on technical details',
      'Practice articulating thought process',
      'Work on answer structure',
      'Improve time management'
    ];

    const count = score < 50 ? 3 : score < 70 ? 2 : 1;
    return allImprovements.slice(0, count);
  }

  private saveCurrentSession(session: InterviewSession): void {
    if (!isPlatformBrowser(this.platformId)) return;
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(session));
  }

  private clearCurrentSession(): void {
    if (!isPlatformBrowser(this.platformId)) return;
    localStorage.removeItem(this.STORAGE_KEY);
  }

  private generateId(): string {
    return `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
  }
}
