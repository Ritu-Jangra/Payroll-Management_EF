import { Component } from '@angular/core';
import { BehaviorSubject, map, switchMap, tap } from 'rxjs';
import { AsyncPipe, DatePipe, NgIf } from '@angular/common';
import { IQuery } from '../../../models/Interfaces/IQuery';
import { LeaveService } from '../../../core/services/Leave/leave.service';
import { ILeave } from '../../../models/Interfaces/ILeaves';

@Component({
  selector: 'app-leaves-requests',
  standalone: true,
  imports: [AsyncPipe, DatePipe, NgIf],
  templateUrl: './leave-requests.component.html',
  styleUrls: ['./leave-requests.component.css'],
})
export class LeaveRequestsComponent {

  message = '';
  hasNextPage = false;

  querySubject = new BehaviorSubject<IQuery>({
    pageNum: 1,
    pageSize: 3,
    filterOn: '',
    filterQuery: '',
    sortBy: '',
    isAscending: true,
  });

  leaves$ = this.querySubject.pipe(
    switchMap((query) =>
      this.leaveService.getTeamLeaves(query).pipe(
        map((leaves) => {
          const safeLeaves = leaves ?? [];

          return safeLeaves.map((l) => ({
            ...l,
            startDate: l.startDate,
            endDate: l.endDate,
          }));
        }),
        tap((leaves) => {
          this.hasNextPage =
            leaves.length >= this.querySubject.value.pageSize;
        })
      )
    )
  );

  constructor(private leaveService: LeaveService) {}

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

  approveLeave(leave: ILeave) {
    this.leaveService.approveLeave(leave).subscribe(() => {
      this.message = 'Leave approved successfully';
      this.updateQuery({ pageNum: 1 });
    });
  }

  rejectLeave(leave: ILeave) {
    this.leaveService.rejectLeave(leave).subscribe(() => {
      this.message = 'Leave rejected successfully';
      this.updateQuery({ pageNum: 1 });
    });
  }
}