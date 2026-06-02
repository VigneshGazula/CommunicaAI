import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'interview/setup',
    loadComponent: () =>
      import('./features/interview/setup/setup.component').then(m => m.SetupComponent),
    canActivate: [authGuard]
  },
  {
    path: 'interview/live/:sessionId',
    loadComponent: () =>
      import('./features/interview/live/live.component').then(m => m.LiveComponent),
    canActivate: [authGuard]
  },
  {
    path: 'interview/result/:sessionId',
    loadComponent: () =>
      import('./features/interview/result/result.component').then(m => m.ResultComponent),
    canActivate: [authGuard]
  },
  {
    path: 'history',
    loadComponent: () =>
      import('./features/interview/history/history.component').then(m => m.HistoryComponent),
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
