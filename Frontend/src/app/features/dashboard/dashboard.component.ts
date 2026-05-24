import { Component, inject, OnInit, signal, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { UserProfile } from '../../core/models/auth.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly platformId = inject(PLATFORM_ID);

  readonly user = signal<UserProfile | null>(null);
  readonly error = signal('');

  ngOnInit(): void {
    // Skip API calls during SSR — token lives in the browser only.
    if (!isPlatformBrowser(this.platformId)) return;

    this.auth.me().subscribe({
      next: (profile) => this.user.set(profile),
      error: () => this.logout()
    });
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
