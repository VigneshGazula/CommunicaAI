import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, UserProfile } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly base = `${environment.apiBaseUrl}/api/auth`;

  register(formData: FormData): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/register`, formData);
  }

  loginPassword(payload: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/login/password`, payload);
  }

  loginAudio(formData: FormData): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/login/audio`, formData);
  }

  loginVideo(formData: FormData): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/login/video`, formData);
  }

  saveTokenSync(token: string): void {
    if (!isPlatformBrowser(this.platformId)) return;
    localStorage.setItem('token', token);
  }

  me(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.base}/me`);
  }

  getToken(): string | null {
    if (!isPlatformBrowser(this.platformId)) return null;
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    if (!isPlatformBrowser(this.platformId)) return;
    localStorage.removeItem('token');
  }
}
