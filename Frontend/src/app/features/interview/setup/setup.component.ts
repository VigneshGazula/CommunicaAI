import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { InterviewService } from '../../../core/services/interview.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-interview-setup',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './setup.component.html',
  styleUrl: './setup.component.scss'
})
export class SetupComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly interviewService = inject(InterviewService);
  private readonly router = inject(Router);

  readonly loading = signal(false);
  readonly error = signal('');
  readonly metadataLoading = signal(true);
  readonly metadataError = signal('');

  readonly roles = signal<string[]>([]);
  readonly difficulties = signal<string[]>([]);

  readonly setupForm = this.fb.nonNullable.group({
    role: ['', Validators.required],
    topic: ['Technical Interview', Validators.required],
    difficulty: ['' as 'easy' | 'medium' | 'hard', Validators.required],
    duration: [15, [Validators.required, Validators.min(5), Validators.max(60)]],
    questionCount: [5, [Validators.required, Validators.min(1), Validators.max(20)]]
  });

  ngOnInit(): void {
    this.loadMetadata();
  }

  private loadMetadata(): void {
    this.metadataLoading.set(true);
    this.metadataError.set('');

    this.interviewService.getMetadata().subscribe({
      next: (metadata) => {
        this.roles.set(metadata.roles);
        this.difficulties.set(metadata.difficulties);
        
        // Set default values after loading
        if (metadata.roles.length > 0) {
          this.setupForm.patchValue({ role: metadata.roles[0] });
        }
        if (metadata.difficulties.length > 0) {
          const mediumDiff = metadata.difficulties.find(d => d.toLowerCase() === 'medium');
          this.setupForm.patchValue({ difficulty: (mediumDiff || metadata.difficulties[0]).toLowerCase() as 'easy' | 'medium' | 'hard' });
        }
        
        this.metadataLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load metadata:', err);
        this.metadataError.set('Failed to load interview options. Please refresh the page.');
        this.metadataLoading.set(false);
      }
    });
  }

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
