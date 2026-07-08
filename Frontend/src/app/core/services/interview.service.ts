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
  SubmitAudioAnswerResponse,
  InterviewHistoryResponse,
  InterviewMetadata,
  CompanyProfile,
  UploadResumeResponse,
  ResumeProfile,
  PerformanceAnalyticsResponse,
  InterviewTypesResponse
} from '../models/interview.models';

@Injectable({ providedIn: 'root' })
export class InterviewService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiBaseUrl}/api/interviews`;
  private readonly questionBankUrl = `${environment.apiBaseUrl}/api/question-bank`;
  private readonly companyUrl = `${environment.apiBaseUrl}/api/company`;
  private readonly resumeUrl = `${environment.apiBaseUrl}/api/resume`;
  private readonly analyticsUrl = `${environment.apiBaseUrl}/api/analytics`;
  
  // Store current session in memory (not localStorage)
  private currentSessionSubject = new BehaviorSubject<InterviewSession | null>(null);
  public currentSession$ = this.currentSessionSubject.asObservable();

  createSession(setup: InterviewSetup, companyProfileId?: string, resumeProfileId?: string, interviewType?: string): Observable<InterviewSession> {
    const request: CreateInterviewRequest = {
      role: setup.role,
      topic: setup.topic,
      difficulty: setup.difficulty,
      questionCount: setup.questionCount,
      durationMinutes: setup.duration,
      companyProfileId: companyProfileId,
      resumeProfileId: resumeProfileId,
      interviewType: interviewType || 'Technical' // Module 9: Default to Technical
    };

    return this.http.post<CreateInterviewResponse>(this.apiUrl, request).pipe(
      map(response => {
        const session: InterviewSession = {
          id: response.sessionId,
          setup: setup,
          questions: [],
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

  getCurrentSession(): InterviewSession | null {
    return this.currentSessionSubject.value;
  }

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

  saveTranscript(sessionId: string, questionId: string, transcript: string): Observable<void> {
    const session = this.currentSessionSubject.value;
    
    if (!session || session.id !== sessionId) {
      return throwError(() => new Error('Session not found'));
    }

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
        const session = this.currentSessionSubject.value;
        if (session && session.id === sessionId) {
          session.answers = session.answers.filter(a => a.questionId !== questionId);
          
          const answer: InterviewAnswer = {
            questionId,
            text: response.transcript,
            timestamp: new Date(),
            audioUrl: response.audioUrl,
            evaluation: {
              // Technical Evaluation
              technicalScore: response.technicalScore,
              clarityScore: response.clarityScore,
              completenessScore: response.completenessScore,
              overallScore: response.overallScore,
              // AI Communication Evaluation
              communicationScore: response.communicationScore,
              confidenceScore: response.confidenceScore,
              grammarScore: response.grammarScore,
              vocabularyScore: response.vocabularyScore,
              professionalismScore: response.professionalismScore,
              answerStructureScore: response.answerStructureScore,
              persuasivenessScore: response.persuasivenessScore,
              concisenessScore: response.concisenessScore,
              // Feedback
              strengths: response.strengths,
              improvements: response.improvements,
              feedback: response.feedback
            }
          };
          session.answers.push(answer);

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

  clearCurrentSession(): void {
    this.currentSessionSubject.next(null);
  }

  getUserHistory(): Observable<InterviewHistoryResponse[]> {
    return this.http.get<InterviewHistoryResponse[]>(`${this.apiUrl}/my-history`).pipe(
      catchError(error => {
        console.error('Error loading interview history:', error);
        return throwError(() => error);
      })
    );
  }

  getMetadata(): Observable<InterviewMetadata> {
    return this.http.get<InterviewMetadata>(`${this.questionBankUrl}/metadata`).pipe(
      catchError(error => {
        console.error('Error loading interview metadata:', error);
        return throwError(() => error);
      })
    );
  }

  // Module 6: Company Intelligence
  getCompanyProfiles(): Observable<CompanyProfile[]> {
    return this.http.get<CompanyProfile[]>(`${this.companyUrl}/profiles`).pipe(
      catchError(error => {
        console.error('Error loading company profiles:', error);
        return throwError(() => error);
      })
    );
  }

  // Module 7: Resume Intelligence
  uploadResume(file: File): Observable<UploadResumeResponse> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<UploadResumeResponse>(`${this.resumeUrl}/upload`, formData).pipe(
      catchError(error => {
        console.error('Error uploading resume:', error);
        return throwError(() => error);
      })
    );
  }

  getLatestResume(): Observable<ResumeProfile> {
    return this.http.get<ResumeProfile>(`${this.resumeUrl}/latest`).pipe(
      catchError(error => {
        console.error('Error loading latest resume:', error);
        return throwError(() => error);
      })
    );
  }

  getMyResumes(): Observable<ResumeProfile[]> {
    return this.http.get<ResumeProfile[]>(`${this.resumeUrl}/my-resumes`).pipe(
      catchError(error => {
        console.error('Error loading resumes:', error);
        return throwError(() => error);
      })
    );
  }

  // Module 8: Performance Analytics
  getPerformanceAnalytics(): Observable<PerformanceAnalyticsResponse> {
    return this.http.get<PerformanceAnalyticsResponse>(`${this.analyticsUrl}/performance`).pipe(
      catchError(error => {
        console.error('Error loading performance analytics:', error);
        return throwError(() => error);
      })
    );
  }

  // Module 9: Specialized Interview Modes
  getInterviewTypes(): Observable<InterviewTypesResponse> {
    return this.http.get<InterviewTypesResponse>(`${this.apiUrl}/types`).pipe(
      catchError(error => {
        console.error('Error loading interview types:', error);
        return throwError(() => error);
      })
    );
  }

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
      currentQuestionIndex: 0,
      interviewType: response.interviewType || 'Technical', // Module 9
      result: response.result ? {
        overallScore: response.result.overallScore ?? 0,
        technicalScore: response.result.technicalScore ?? 0,
        communicationScore: response.result.communicationScore ?? 0,
        confidenceScore: response.result.confidenceScore ?? 0,
        strengths: response.result.strengths ?? '',
        weaknesses: response.result.weaknesses ?? '',
        recommendations: response.result.recommendations ?? '',
        summary: response.result.summary ?? ''
      } : undefined
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
