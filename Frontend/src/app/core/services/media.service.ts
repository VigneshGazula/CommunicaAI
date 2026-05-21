import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MediaItem } from '../models/media.models';

@Injectable({ providedIn: 'root' })
export class MediaService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiBaseUrl}/api/media`;

  submitOnboarding(formData: FormData): Observable<void> {
    return this.http.post<void>(`${this.base}/onboarding`, formData);
  }

  getMyMedia(): Observable<MediaItem[]> {
    return this.http.get<MediaItem[]>(`${this.base}/me`);
  }
}
