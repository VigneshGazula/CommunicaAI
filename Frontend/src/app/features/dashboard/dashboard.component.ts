import { Component, inject, signal, afterNextRender } from '@angular/core';
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
export class DashboardComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly user = signal<UserProfile | null>(null);
  readonly loading = signal(true);
  readonly error = signal('');

  constructor() {
    // afterNextRender only runs in the browser, never during SSR.
    // This guarantees localStorage is available and the token is readable.
    afterNextRender(() => {
      this.auth.me().subscribe({
        next: (profile) => {
          this.user.set(profile);
          this.loading.set(false);
        },
        error: () => {
          this.auth.logout();
          this.router.navigate(['/login']);
        }
      });
    });
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
