import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { InterviewService } from '../../../core/services/interview.service';

@Component({
  selector: 'app-interview-setup',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './setup.component.html',
  styleUrl: './setup.component.scss'
})
export class SetupComponent {
  private readonly fb = inject(FormBuilder);
  private readonly interviewService = inject(InterviewService);
  private readonly router = inject(Router);

  readonly loading = signal(false);
  readonly error = signal('');

  readonly setupForm = this.fb.nonNullable.group({
    role: ['Software Engineer', Validators.required],
    topic: ['Technical Interview', Validators.required],
    difficulty: ['medium' as 'easy' | 'medium' | 'hard', Validators.required],
    duration: [15, [Validators.required, Validators.min(5), Validators.max(60)]],
    questionCount: [5, [Validators.required, Validators.min(1), Validators.max(20)]]
  });

  readonly roles = [
    'Software Engineer',
    'Product Manager',
    'Data Scientist',
    'Marketing Manager',
    'UX Designer',
    'Business Analyst',
    'Sales Executive',
    'Customer Success Manager'
  ];

  submit(): void {
    if (this.setupForm.invalid) {
      this.setupForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set('');

    const setup = this.setupForm.getRawValue();

    this.interviewService.createSession(setup).subscribe({
      next: (session) => {
        this.router.navigate(['/interview/live', session.id]);
      },
      error: () => {
        this.error.set('Failed to create interview session. Please try again.');
        this.loading.set(false);
      }
    });
  }
}
