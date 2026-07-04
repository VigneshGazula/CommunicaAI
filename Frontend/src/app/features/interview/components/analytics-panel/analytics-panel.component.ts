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
  readonly voiceIntel = this.analytics.voiceIntelligence;
  
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

  readonly paceRatingColor = computed(() => {
    const rating = this.voiceIntel().paceRating;
    switch (rating) {
      case 'Too Slow': return '#ef4444';
      case 'Good': return '#10b981';
      case 'Excellent': return '#10b981';
      case 'Too Fast': return '#f59e0b';
      default: return '#6b7280';
    }
  });

  readonly fluencyColor = computed(() => {
    const score = this.voiceIntel().fluencyScore;
    if (score >= 80) return '#10b981';
    if (score >= 60) return '#f59e0b';
    return '#ef4444';
  });

  readonly communicationColor = computed(() => {
    const score = this.voiceIntel().communicationScore;
    if (score >= 80) return '#10b981';
    if (score >= 60) return '#f59e0b';
    return '#ef4444';
  });

  readonly energyColor = computed(() => {
    const energy = this.voiceIntel().voiceEnergy;
    if (energy >= 70) return '#10b981';
    if (energy >= 40) return '#f59e0b';
    return '#ef4444';
  });

  readonly showVoiceIntelligence = computed(() => {
    // Show voice intelligence after recording stops or during recording if metrics available
    const state = this.metrics().recordingState;
    return state === 'stopped' || (state === 'active' && this.metrics().recordingDuration > 5);
  });
}
