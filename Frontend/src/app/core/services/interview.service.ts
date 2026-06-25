import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap, map, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  InterviewSetup,
  InterviewSession,
  InterviewQuestion,
  InterviewAnswer,
  CreateInterviewRequest,
  CreateInterviewResponse,
  QuestionResponse,
  InterviewDetailResponse,
  QuestionWithAnswerResponse,
  SubmitAudioAnswerResponse
} from '../models/interview.models';

@Injectable({ providedIn: 'root' })
export class InterviewService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/api/interviews`;
  
  // Store current session in memory (not localStorage)
  private currentSessionSubject = new BehaviorSubject<InterviewSession | null>(null);
  public currentSession$ = this.currentSessionSubject.asObservable();

  /**
   * Create a new interview session
   * Calls: POST /api/interviews
   */
  createSession(setup: InterviewSetup): Observable<InterviewSession> {
    const request: CreateInterviewRequest = {
      role: setup.role,
      topic: setup.topic,
      difficulty: setup.difficulty,
      questionCount: setup.questionCount,
      durationMinutes: setup.duration
    };

    return this.http.post<CreateInterviewResponse>(this.apiUrl, request).pipe(
      map(response => {
        // Transform backend response to frontend session model
        const session: InterviewSession = {
          id: response.sessionId,
          setup: setup,
          questions: [], // Will be loaded separately
          answers: [],
          status: 'in-progress',
          createdAt: new Date(response.startedAt),
          currentQuestionIndex: 0
        };
        
        this.currentSessionSubject.next(session);
        return session;
      }),
      catchError(error => {
        console.error('Error creating interview session:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Get current session from memory
   */
  getCurrentSession(): InterviewSession | null {
    return this.currentSessionSubject.value;
  }

  /**
   * Load full interview session details
   * Calls: GET /api/interviews/{sessionId}
   */
  loadSessionDetails(sessionId: string): Observable<InterviewSession> {
    return this.http.get<InterviewDetailResponse>(`${this.apiUrl}/${sessionId}`).pipe(
      map(response => this.mapDetailResponseToSession(response)),
      tap(session => this.currentSessionSubject.next(session)),
      catchError(error => {
        console.error('Error loading session details:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Load questions for a session
   * Calls: GET /api/interviews/{sessionId}/questions
   */
  loadQuestions(sessionId: string): Observable<InterviewQuestion[]> {
    return this.http.get<QuestionResponse[]>(`${this.apiUrl}/${sessionId}/questions`).pipe(
      map(responses => responses.map(q => this.mapQuestionResponse(q))),
      tap(questions => {
        const currentSession = this.currentSessionSubject.value;
        if (currentSession && currentSession.id === sessionId) {
          currentSession.questions = questions;
          this.currentSessionSubject.next({ ...currentSession });
        }
      }),
      catchError(error => {
        console.error('Error loading questions:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Save answer transcript (updates local state only)
   * Note: Backend submission happens via InterviewAnswerController
   */
  saveTranscript(sessionId: string, questionId: string, transcript: string): Observable<void> {
    const session = this.currentSessionSubject.value;
    
    if (!session || session.id !== sessionId) {
      return throwError(() => new Error('Session not found'));
    }

    // Update or create answer in local state
    const existingAnswerIndex = session.answers.findIndex(a => a.questionId === questionId);
    const answer: InterviewAnswer = {
      questionId,
      text: transcript,
      timestamp: new Date()
    };

    if (existingAnswerIndex >= 0) {
      session.answers[existingAnswerIndex] = answer;
    } else {
      session.answers.push(answer);
    }

    // Update question's isAnswered flag
    const question = session.questions.find(q => q.id === questionId);
    if (question) {
      question.isAnswered = true;
    }

    this.currentSessionSubject.next({ ...session });
    
    return new Observable(observer => {
      observer.next();
      observer.complete();
    });
  }

  /**
   * Update current question index in local state
   */
  updateQuestionIndex(sessionId: string, index: number): Observable<void> {
    const session = this.currentSessionSubject.value;
    
    if (!session || session.id !== sessionId) {
      return throwError(() => new Error('Session not found'));
    }

    session.currentQuestionIndex = index;
    this.currentSessionSubject.next({ ...session });
    
    return new Observable(observer => {
      observer.next();
      observer.complete();
    });
  }

  /**
   * Complete the interview session
   * Calls: POST /api/interviews/{sessionId}/complete
   */
  completeInterview(sessionId: string): Observable<void> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/${sessionId}/complete`, {}).pipe(
      tap(() => {
        const session = this.currentSessionSubject.value;
        if (session && session.id === sessionId) {
          session.status = 'completed';
          session.completedAt = new Date();
          this.currentSessionSubject.next({ ...session });
        }
      }),
      map(() => void 0),
      catchError(error => {
        console.error('Error completing interview:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Submit audio answer with transcription and evaluation
   * Calls: POST /api/interviews/{sessionId}/answers/audio
   */
  submitAudioAnswer(
    sessionId: string,
    questionId: string,
    audioBlob: Blob,
    durationSeconds: number
  ): Observable<SubmitAudioAnswerResponse> {
    const formData = new FormData();
    formData.append('questionId', questionId);
    formData.append('audioFile', audioBlob, 'answer.webm');
    formData.append('durationSeconds', durationSeconds.toString());

    return this.http.post<SubmitAudioAnswerResponse>(
      `${this.apiUrl}/${sessionId}/answers/audio`,
      formData
    ).pipe(
      tap(response => {
        // Update local session state with answer
        const session = this.currentSessionSubject.value;
        if (session && session.id === sessionId) {
          // Remove existing answer for this question
          session.answers = session.answers.filter(a => a.questionId !== questionId);
          
          // Add new answer with evaluation
          const answer: InterviewAnswer = {
            questionId,
            text: response.transcript,
            timestamp: new Date(),
            audioUrl: response.audioUrl,
            evaluation: {
              technicalScore: response.technicalScore,
              clarityScore: response.clarityScore,
              completenessScore: response.completenessScore,
              overallScore: response.overallScore,
              strengths: response.strengths,
              improvements: response.improvements,
              feedback: response.feedback
            }
          };
          session.answers.push(answer);

          // Mark question as answered
          const question = session.questions.find(q => q.id === questionId);
          if (question) {
            question.isAnswered = true;
          }

          this.currentSessionSubject.next({ ...session });
        }
      }),
      catchError(error => {
        console.error('Error submitting audio answer:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Clear current session from memory
   */
  clearCurrentSession(): void {
    this.currentSessionSubject.next(null);
  }

  // Helper method to map backend response to frontend model
  private mapDetailResponseToSession(response: InterviewDetailResponse): InterviewSession {
    const questions = response.questions.map(q => this.mapQuestionWithAnswer(q));
    const answers = response.questions
      .filter(q => q.answer !== null)
      .map(q => ({
        questionId: q.id,
        text: q.answer!.transcript,
        timestamp: new Date(q.answer!.answeredAt)
      }));

    return {
      id: response.sessionId,
      setup: {
        role: response.role,
        topic: response.topic,
        difficulty: response.difficulty as 'easy' | 'medium' | 'hard',
        duration: response.durationMinutes,
        questionCount: response.questionCount
      },
      questions,
      answers,
      status: response.status.toLowerCase() as 'draft' | 'in-progress' | 'completed',
      createdAt: new Date(response.startedAt),
      completedAt: response.completedAt ? new Date(response.completedAt) : undefined,
      currentQuestionIndex: 0
    };
  }

  private mapQuestionResponse(q: QuestionResponse): InterviewQuestion {
    return {
      id: q.id,
      text: q.questionText,
      order: q.orderNumber,
      category: q.category,
      isAnswered: q.isAnswered
    };
  }

  private mapQuestionWithAnswer(q: QuestionWithAnswerResponse): InterviewQuestion {
    return {
      id: q.id,
      text: q.questionText,
      order: q.orderNumber,
      category: q.category,
      isAnswered: q.isAnswered
    };
  }
}
