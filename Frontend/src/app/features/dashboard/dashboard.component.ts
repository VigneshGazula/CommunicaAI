import { Component, inject, signal, OnInit, computed, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { DatePipe, TitleCasePipe } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';
import { InterviewService } from '../../core/services/interview.service';
import { UserProfile } from '../../core/models/auth.models';
import { InterviewHistoryResponse, PerformanceAnalyticsResponse } from '../../core/models/interview.models';
import { forkJoin } from 'rxjs';
import { Chart, ChartConfiguration, registerables } from 'chart.js';

Chart.register(...registerables);

interface DashboardSession {
  sessionId: string;
  role: string;
  difficulty: string;
  status: string;
  startedAt: Date;
  completedAt: Date | null;
  completionPercentage: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink, DatePipe, TitleCasePipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, AfterViewInit {
  @ViewChild('technicalChartCanvas') technicalChartCanvas?: ElementRef<HTMLCanvasElement>;
  @ViewChild('communicationChartCanvas') communicationChartCanvas?: ElementRef<HTMLCanvasElement>;
  @ViewChild('confidenceChartCanvas') confidenceChartCanvas?: ElementRef<HTMLCanvasElement>;
  @ViewChild('skillsChartCanvas') skillsChartCanvas?: ElementRef<HTMLCanvasElement>;
  private readonly auth = inject(AuthService);
  private readonly interviewService = inject(InterviewService);
  private readonly router = inject(Router);

  readonly user = signal<UserProfile | null>(null);
  readonly history = signal<InterviewHistoryResponse[]>([]);
  readonly analytics = signal<PerformanceAnalyticsResponse | null>(null);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly showAnalytics = signal(false);

  private technicalChart?: Chart;
  private communicationChart?: Chart;
  private confidenceChart?: Chart;
  private skillsChart?: Chart;

  // Computed values from history
  readonly totalInterviews = computed(() => this.history().length);
  
  readonly completedInterviews = computed(() => 
    this.history().filter(h => h.status.toLowerCase() === 'completed').length
  );

  readonly averageScore = computed(() => {
    const completed = this.history().filter(h => 
      h.status.toLowerCase() === 'completed' && h.completionPercentage !== null
    );
    
    if (completed.length === 0) return 0;
    
    const sum = completed.reduce((acc, h) => acc + (h.completionPercentage || 0), 0);
    return Math.round(sum / completed.length);
  });

  readonly recentSessions = computed(() => {
    return this.history()
      .filter(h => h.completedAt !== null)
      .sort((a, b) => new Date(b.completedAt!).getTime() - new Date(a.completedAt!).getTime())
      .slice(0, 3)
      .map(h => ({
        sessionId: h.sessionId,
        role: h.role,
        difficulty: h.difficulty,
        status: h.status,
        startedAt: new Date(h.startedAt),
        completedAt: h.completedAt ? new Date(h.completedAt) : null,
        completionPercentage: h.completionPercentage || 0
      }));
  });

  readonly currentStreak = computed(() => {
    const completed = this.history()
      .filter(h => h.status.toLowerCase() === 'completed' && h.completedAt !== null)
      .sort((a, b) => new Date(b.completedAt!).getTime() - new Date(a.completedAt!).getTime());

    if (completed.length === 0) return 0;

    let streak = 0;
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    for (const session of completed) {
      const sessionDate = new Date(session.completedAt!);
      sessionDate.setHours(0, 0, 0, 0);
      
      const daysDiff = Math.floor((today.getTime() - sessionDate.getTime()) / (1000 * 60 * 60 * 24));
      
      if (daysDiff <= streak + 1) {
        streak++;
      } else {
        break;
      }
    }

    return streak;
  });

  ngOnInit(): void {
    this.loadDashboardData();
  }

  ngAfterViewInit(): void {
    // Charts will be initialized after analytics data is loaded
  }

  private loadDashboardData(): void {
    forkJoin({
      user: this.auth.me(),
      history: this.interviewService.getUserHistory(),
      analytics: this.interviewService.getPerformanceAnalytics()
    }).subscribe({
      next: ({ user, history, analytics }) => {
        this.user.set(user);
        this.history.set(history);
        this.analytics.set(analytics);
        this.loading.set(false);
        
        // Initialize charts after data is loaded
        setTimeout(() => this.initializeCharts(), 100);
      },
      error: (err) => {
        console.error('Error loading dashboard data:', err);
        this.error.set('Failed to load dashboard data');
        this.loading.set(false);
        
        // If unauthorized, redirect to login
        if (err.status === 401) {
          this.auth.logout();
          this.router.navigate(['/login']);
        }
      }
    });
  }

  toggleAnalytics(): void {
    this.showAnalytics.set(!this.showAnalytics());
    if (this.showAnalytics()) {
      setTimeout(() => this.initializeCharts(), 100);
    }
  }

  private initializeCharts(): void {
    const analyticsData = this.analytics();
    if (!analyticsData) return;

    this.initializeTechnicalChart(analyticsData);
    this.initializeCommunicationChart(analyticsData);
    this.initializeConfidenceChart(analyticsData);
    this.initializeSkillsChart(analyticsData);
  }

  private initializeTechnicalChart(data: PerformanceAnalyticsResponse): void {
    if (!this.technicalChartCanvas?.nativeElement) return;

    if (this.technicalChart) {
      this.technicalChart.destroy();
    }

    const ctx = this.technicalChartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const config: ChartConfiguration = {
      type: 'line',
      data: {
        labels: data.technicalScoreTrend.map(t => new Date(t.date).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })),
        datasets: [{
          label: 'Technical Score',
          data: data.technicalScoreTrend.map(t => t.score),
          borderColor: 'rgb(108, 71, 255)',
          backgroundColor: 'rgba(108, 71, 255, 0.1)',
          tension: 0.4,
          fill: true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            max: 100
          }
        }
      }
    };

    this.technicalChart = new Chart(ctx, config);
  }

  private initializeCommunicationChart(data: PerformanceAnalyticsResponse): void {
    if (!this.communicationChartCanvas?.nativeElement) return;

    if (this.communicationChart) {
      this.communicationChart.destroy();
    }

    const ctx = this.communicationChartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const config: ChartConfiguration = {
      type: 'line',
      data: {
        labels: data.communicationScoreTrend.map(t => new Date(t.date).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })),
        datasets: [{
          label: 'Communication Score',
          data: data.communicationScoreTrend.map(t => t.score),
          borderColor: 'rgb(34, 197, 94)',
          backgroundColor: 'rgba(34, 197, 94, 0.1)',
          tension: 0.4,
          fill: true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            max: 100
          }
        }
      }
    };

    this.communicationChart = new Chart(ctx, config);
  }

  private initializeConfidenceChart(data: PerformanceAnalyticsResponse): void {
    if (!this.confidenceChartCanvas?.nativeElement) return;

    if (this.confidenceChart) {
      this.confidenceChart.destroy();
    }

    const ctx = this.confidenceChartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const config: ChartConfiguration = {
      type: 'line',
      data: {
        labels: data.confidenceScoreTrend.map(t => new Date(t.date).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })),
        datasets: [{
          label: 'Confidence Score',
          data: data.confidenceScoreTrend.map(t => t.score),
          borderColor: 'rgb(245, 158, 11)',
          backgroundColor: 'rgba(245, 158, 11, 0.1)',
          tension: 0.4,
          fill: true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            max: 100
          }
        }
      }
    };

    this.confidenceChart = new Chart(ctx, config);
  }

  private initializeSkillsChart(data: PerformanceAnalyticsResponse): void {
    if (!this.skillsChartCanvas?.nativeElement) return;

    if (this.skillsChart) {
      this.skillsChart.destroy();
    }

    const ctx = this.skillsChartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const strongestSkills = data.strongestSkills.slice(0, 5);
    const weakestSkills = data.weakestSkills.slice(0, 5);

    const config: ChartConfiguration = {
      type: 'bar',
      data: {
        labels: [...strongestSkills.map(s => s.skillName), ...weakestSkills.map(s => s.skillName)],
        datasets: [{
          label: 'Skill Score',
          data: [...strongestSkills.map(s => s.averageScore), ...weakestSkills.map(s => s.averageScore)],
          backgroundColor: [
            ...strongestSkills.map(() => 'rgba(34, 197, 94, 0.7)'),
            ...weakestSkills.map(() => 'rgba(239, 68, 68, 0.7)')
          ],
          borderColor: [
            ...strongestSkills.map(() => 'rgb(34, 197, 94)'),
            ...weakestSkills.map(() => 'rgb(239, 68, 68)')
          ],
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        indexAxis: 'y',
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          x: {
            beginAtZero: true,
            max: 100
          }
        }
      }
    };

    this.skillsChart = new Chart(ctx, config);
  }

  startInterview(): void {
    this.router.navigate(['/interview/setup']);
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
