import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { InterviewService } from '../../../core/services/interview.service';
import { CompanyProfile, ResumeMetadata } from '../../../core/models/interview.models';
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
  readonly companies = signal<CompanyProfile[]>([]);
  readonly uploadedResume = signal<ResumeMetadata | null>(null);
  readonly resumeUploading = signal(false);
  readonly resumeError = signal('');
  readonly uploadedResumeId = signal<string | null>(null);

  readonly setupForm = this.fb.nonNullable.group({
    role: ['', Validators.required],
    topic: ['Technical Interview', Validators.required],
    difficulty: ['' as 'easy' | 'medium' | 'hard', Validators.required],
    duration: [15, [Validators.required, Validators.min(5), Validators.max(60)]],
    questionCount: [5, [Validators.required, Validators.min(1), Validators.max(20)]],
    companyProfileId: [''],
    resumeProfileId: ['']
  });

  ngOnInit(): void {
    this.loadMetadata();
    this.loadCompanyProfiles();
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

  private loadCompanyProfiles(): void {
    this.interviewService.getCompanyProfiles().subscribe({
      next: (profiles) => {
        this.companies.set(profiles);
      },
      error: (err) => {
        console.error('Failed to load company profiles:', err);
      }
    });
  }

  onResumeSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      // Validate file type
      const allowedTypes = ['application/pdf', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
      if (!allowedTypes.includes(file.type)) {
        this.resumeError.set('Only PDF and DOCX files are supported');
        return;
      }

      // Validate file size (5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.resumeError.set('File size must be less than 5MB');
        return;
      }

      this.uploadResume(file);
    }
  }

  private uploadResume(file: File): void {
    this.resumeUploading.set(true);
    this.resumeError.set('');

    this.interviewService.uploadResume(file).subscribe({
      next: (response) => {
        this.uploadedResume.set(response.metadata);
        this.uploadedResumeId.set(response.resumeId);
        this.setupForm.patchValue({ resumeProfileId: response.resumeId });
        this.resumeUploading.set(false);
      },
      error: (err) => {
        console.error('Resume upload failed:', err);
        this.resumeError.set(err.error?.message || 'Failed to upload resume. Please try again.');
        this.resumeUploading.set(false);
      }
    });
  }

  clearResume(): void {
    this.uploadedResume.set(null);
    this.uploadedResumeId.set(null);
    this.setupForm.patchValue({ resumeProfileId: '' });
    this.resumeError.set('');
  }

  submit(): void {
    if (this.setupForm.invalid) {
      this.setupForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set('');

    const formValue = this.setupForm.getRawValue();
    const setup = {
      role: formValue.role,
      topic: formValue.topic,
      difficulty: formValue.difficulty,
      duration: formValue.duration,
      questionCount: formValue.questionCount
    };
    const companyProfileId = formValue.companyProfileId || undefined;
    const resumeProfileId = formValue.resumeProfileId || undefined;

    this.interviewService.createSession(setup, companyProfileId, resumeProfileId).subscribe({
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
