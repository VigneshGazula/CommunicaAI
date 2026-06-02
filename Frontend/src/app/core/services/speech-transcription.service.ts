import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

export interface TranscriptionResult {
  text: string;
  confidence: number;
  timestamp: Date;
}

@Injectable({ providedIn: 'root' })
export class SpeechTranscriptionService {
  /**
   * Transcribe audio blob to text.
   * Mock implementation returns placeholder text.
   * Replace with real Whisper API call when backend is ready.
   */
  transcribe(audioBlob: Blob): Observable<TranscriptionResult> {
    // Mock transcription with simulated delay
    const mockText = this.generateMockTranscription(audioBlob.size);
    
    return of({
      text: mockText,
      confidence: 0.95,
      timestamp: new Date()
    }).pipe(delay(800)); // Simulate network delay
  }

  private generateMockTranscription(blobSize: number): string {
    const templates = [
      'I have extensive experience in this area and have worked on several projects that demonstrate my capabilities.',
      'My approach involves careful analysis, strategic planning, and collaborative execution with cross-functional teams.',
      'I believe the key to success in this role is maintaining strong communication and focusing on measurable outcomes.',
      'In my previous position, I developed solutions that improved efficiency and delivered significant value to stakeholders.',
      'I would handle this situation by first gathering all relevant information, then consulting with team members before making a decision.'
    ];

    // Return random template based on blob size
    const index = Math.floor(Math.random() * templates.length);
    return templates[index];
  }

  /**
   * Future implementation placeholder for real Whisper integration:
   * 
   * transcribe(audioBlob: Blob): Observable<TranscriptionResult> {
   *   const formData = new FormData();
   *   formData.append('audio', audioBlob, 'recording.webm');
   *   
   *   return this.http.post<TranscriptionResult>(
   *     `${environment.apiBaseUrl}/api/speech/transcribe`,
   *     formData
   *   );
   * }
   */
}
