import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LeaveService } from '../../../core/services/Leave/leave.service';
import { ReasonType } from '../../../models/Enums/ReasonsType';
import { AsyncPipe, NgIf } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-apply-leave',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, AsyncPipe],
  templateUrl: './apply-leave.component.html',
  styleUrls: ['./apply-leave.component.css'],
})
export class ApplyLeaveComponent {
  leaveForm!: FormGroup;
  reasonType = ReasonType;
  message = '';
  error$ = new BehaviorSubject<string | null>(null);

  constructor(
    private fb: FormBuilder,
    private leaveService: LeaveService,
  ) {}

  ngOnInit() {
    this.leaveForm = this.fb.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      reason: ['', Validators.required],
    });
  }

  applyLeave(leaveForm: FormGroup) {
    if (leaveForm.invalid) return;

    const leaveData = {
      ...leaveForm.value,
      status: 'Pending',
    };
    this.error$.next(null);

    this.leaveService.applyLeave(leaveData).subscribe({
      next: () => {
        this.leaveForm.reset();
        this.message = 'Leave applied successfully!';
        this.error$.next(null);
      },
      error: (err: any) => {
        this.message = '';
        console.error('Leave Application failed:', err.message);
        this.error$.next(err.message || 'Failed to apply for leave. Please try again.');
      },
    });
  }
}
