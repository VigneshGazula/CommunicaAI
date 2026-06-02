import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import { InterviewResult, InterviewStats } from '../models/interview.models';

@Injectable({ providedIn: 'root' })
export class InterviewHistoryService {
  private readonly platformId = inject(PLATFORM_ID);
  private readonly STORAGE_KEY = 'communica_interview_history';

  listSessions(): Observable<InterviewResult[]> {
    const sessions = this.getStoredSessions();
    return of(sessions).pipe(delay(200));
  }

  getSessionById(id: string): Observable<InterviewResult | null> {
    const sessions = this.getStoredSessions();
    const session = sessions.find(s => s.sessionId === id) || null;
    return of(session).pipe(delay(200));
  }

  saveSession(result: InterviewResult): Observable<void> {
    const sessions = this.getStoredSessions();
    sessions.unshift(result); // Add to beginning
    this.setStoredSessions(sessions);
    return of(void 0).pipe(delay(100));
  }

  getStats(): Observable<InterviewStats> {
    const sessions = this.getStoredSessions();
    
    const stats: InterviewStats = {
      totalInterviews: sessions.length,
      averageScore: sessions.length > 0
        ? Math.round(sessions.reduce((sum, s) => sum + s.overallScore, 0) / sessions.length)
        : 0,
      currentStreak: this.calculateStreak(sessions)
    };

    return of(stats).pipe(delay(200));
  }

  clearHistory(): Observable<void> {
    if (!isPlatformBrowser(this.platformId)) return of(void 0);
    localStorage.removeItem(this.STORAGE_KEY);
    return of(void 0);
  }

  private getStoredSessions(): InterviewResult[] {
    if (!isPlatformBrowser(this.platformId)) return [];
    
    const stored = localStorage.getItem(this.STORAGE_KEY);
    if (!stored) return [];
    
    try {
      const sessions = JSON.parse(stored);
      return sessions.map((s: any) => ({
        ...s,
        completedAt: new Date(s.completedAt)
      }));
    } catch {
      return [];
    }
  }

  private setStoredSessions(sessions: InterviewResult[]): void {
    if (!isPlatformBrowser(this.platformId)) return;
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(sessions));
  }

  private calculateStreak(sessions: InterviewResult[]): number {
    if (sessions.length === 0) return 0;

    const sortedSessions = [...sessions].sort(
      (a, b) => b.completedAt.getTime() - a.completedAt.getTime()
    );

    let streak = 0;
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    for (const session of sortedSessions) {
      const sessionDate = new Date(session.completedAt);
      sessionDate.setHours(0, 0, 0, 0);
      
      const daysDiff = Math.floor((today.getTime() - sessionDate.getTime()) / (1000 * 60 * 60 * 24));
      
      if (daysDiff <= streak + 1) {
        streak++;
      } else {
        break;
      }
    }

    return streak;
  }
}
