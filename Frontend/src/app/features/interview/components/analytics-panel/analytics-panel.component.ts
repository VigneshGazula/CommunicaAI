import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InterviewAnalyticsService } from '../../../../core/services/interview-analytics.service';

@Component({
  selector: 'app-analytics-panel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './analytics-panel.component.html',
  styleUrl: './analytics-panel.component.scss'
})
export class AnalyticsPanelComponent {
  private readonly analytics = inject(InterviewAnalyticsService);

  readonly metrics = this.analytics.metrics;
  
  readonly formattedDuration = computed(() => {
    const duration = this.metrics().recordingDuration;
    const minutes = Math.floor(duration / 60);
    const seconds = duration % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  });

  readonly statusColor = computed(() => {
    const status = this.metrics().microphoneStatus;
    switch (status) {
      case 'recording': return '#a855f7';
      case 'listening': return '#10b981';
      case 'processing': return '#f59e0b';
      default: return '#6b7280';
    }
  });

  readonly statusIcon = computed(() => {
    const status = this.metrics().microphoneStatus;
    switch (status) {
      case 'recording': return '🎙️';
      case 'listening': return '👂';
      case 'processing': return '⚙️';
      default: return '⏸️';
    }
  });

  readonly wpmColor = computed(() => {
    const wpm = this.metrics().speakingSpeed;
    if (wpm === 0) return '#6b7280';
    if (wpm < 100) return '#ef4444'; // Too slow
    if (wpm < 150) return '#10b981'; // Good
    if (wpm < 180) return '#10b981'; // Excellent
    return '#f59e0b'; // Too fast
  });

  readonly wpmFeedback = computed(() => {
    const wpm = this.metrics().speakingSpeed;
    if (wpm === 0) return 'Start speaking';
    if (wpm < 100) return 'Speak faster';
    if (wpm < 150) return 'Good pace';
    if (wpm < 180) return 'Excellent!';
    return 'Slow down';
  });
}
