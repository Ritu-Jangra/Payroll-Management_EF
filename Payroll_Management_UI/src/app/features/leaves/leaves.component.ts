import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, map, switchMap } from 'rxjs';
import { LeaveService } from '../../core/services/Leave/leave.service';
import { AsyncPipe, DatePipe, NgFor, NgIf } from '@angular/common';
import { IQuery } from '../../models/Interfaces/IQuery';
import { ILeave } from '../../models/Interfaces/ILeaves';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ReasonType } from '../../models/Enums/ReasonsType';

@Component({
  selector: 'app-leaves',
  standalone: true,
  imports: [AsyncPipe, DatePipe, NgFor, NgIf, ReactiveFormsModule],
  templateUrl: './leaves.component.html',
  styleUrls: ['./leaves.component.css'],
})
export class LeavesComponent implements OnInit {
  selectedLeave: ILeave | null = null;
  isModalOpen = false;
  message = '';
  hasNextPage = false;
  reasonTypes = Object.values(ReasonType);
  querySubject = new BehaviorSubject<IQuery>({
    pageNum: 1,
    pageSize: 3,
    filterOn: '',
    filterQuery: '',
    sortBy: '',
    isAscending: true,
  });
  error$ = new BehaviorSubject<string | null>(null);

  leaves$ = this.querySubject.pipe(
    switchMap((query) =>
      this.leaveService.getMyLeaves(query).pipe(
        map((leaves) => {
          const processed = leaves.map((l) => ({
            ...l,
            startDate: l.startDate,
            endDate: l.endDate,
          }));

          this.hasNextPage = leaves.length === query.pageSize;

          return processed.filter((l) =>
            query.filterQuery
              ? (l.status + ' ' + l.reason).toLowerCase().includes(query.filterQuery.toLowerCase())
              : true,
          );
        }),
      ),
    ),
  );

  leaveForm!: FormGroup;

  constructor(
    private leaveService: LeaveService,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.leaveForm = this.fb.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      reason: ['', Validators.required],
    });
  }

  updateQuery(partial: Partial<IQuery>) {
    this.querySubject.next({
      ...this.querySubject.value,
      ...partial,
    });
  }

  sort(column: string) {
    const current = this.querySubject.value;

    this.updateQuery({
      sortBy: column,
      isAscending: current.sortBy === column ? !current.isAscending : true,
      pageNum: 1,
    });
  }

  updateLeave(leave: ILeave) {
    this.selectedLeave = leave;

    this.leaveForm.patchValue({
      startDate: this.toDateInput(leave.startDate),
      endDate: this.toDateInput(leave.endDate),
      reason: leave.reason,
    });

    this.isModalOpen = true;
  }

  saveLeave() {
    if (!this.selectedLeave || this.leaveForm.invalid) return;

    this.error$.next(null);
    const formValue = this.leaveForm.value;

    const updatedLeave: ILeave = {
      ...this.selectedLeave,
      startDate: new Date(formValue.startDate).toISOString(),
      endDate: new Date(formValue.endDate).toISOString(),
      reason: formValue.reason,
    };

    this.leaveService.updateLeave(updatedLeave).subscribe({
      next: () => {
        this.message = 'Leave updated successfully';
        this.closeModal();
        this.updateQuery({ pageNum: 1 });
      },
      error: (err: any) => {
        this.message = '';
        console.error('Update failed:', err.message);
        this.error$.next(err.message || 'Failed to update leave. Please try again.');
      },
    });
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedLeave = null;
    this.leaveForm.reset();
  }

  cancelLeave(leave: ILeave) {
    this.leaveService.cancelLeave(leave).subscribe(() => {
      this.message = 'Leave cancelled successfully';
      this.updateQuery({ pageNum: 1 });
    });
  }

  private toDateInput(date: string | Date): string {
    return new Date(date).toISOString().split('T')[0];
  }
}
