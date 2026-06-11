import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { catchError, Observable, throwError } from 'rxjs';
import { ILeave } from '../../../models/Interfaces/ILeaves';
import { IQuery } from '../../../models/Interfaces/IQuery';

@Injectable({
  providedIn: 'root',
})
export class LeaveService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMyLeaves(query: IQuery): Observable<ILeave[]> {
    const params = this.handleQueryParams(query);
    return this.safeFallback(
      () => this.http.get<ILeave[]>(`${this.baseUrl}/v2/leaves/my`, { params }),
      () => this.http.get<ILeave[]>(`${this.baseUrl}/v1/leaves/my`, { params }),
    );
  }

  getTeamLeaves(query: IQuery) {
    const params = this.handleQueryParams(query);
    return this.http.get<ILeave[]>(`${this.baseUrl}/v2/leaves/team`, { params });
  }

  applyLeave(leave: ILeave) {
    return this.safeFallback(
      () => this.http.post(`${this.baseUrl}/v2/leaves`, leave),
      () => this.http.post(`${this.baseUrl}/v1/leaves`, leave),
    );
  }

  updateLeave(leave: ILeave) {
    return this.safeFallback(
      () => this.http.put(`${this.baseUrl}/v2/leaves/${leave.leaveId}`, leave),
      () => this.http.put(`${this.baseUrl}/v1/leaves/${leave.leaveId}`, leave),
    );
  }

  cancelLeave(leave: ILeave) {
    return this.safeFallback(
      () => this.http.delete(`${this.baseUrl}/v2/leaves/${leave.leaveId}`, { body: leave }),
      () => this.http.delete(`${this.baseUrl}/v1/leaves/${leave.leaveId}`, { body: leave }),
    );
  }

  approveLeave(leave: ILeave) {
    return this.http.put(`${this.baseUrl}/v2/leaves/${leave.leaveId}/approve`, leave);
  }

  rejectLeave(leave: ILeave) {
    return this.http.put(`${this.baseUrl}/v2/leaves/${leave.leaveId}/reject`, leave);
  }

  private safeFallback<T>(v2Call: () => Observable<T>, v1Call: () => Observable<T>): Observable<T> {
    return v2Call().pipe(
      catchError((err) => {
        if (err.status === 503) {
          return v1Call();
        }
        return throwError(() => err);
      }),
    );
  }

  handleQueryParams(query: IQuery): HttpParams {
    return new HttpParams()
      .set('filterOn', query.filterOn ?? '')
      .set('filterQuery', query.filterQuery ?? '')
      .set('sortBy', query.sortBy ?? '')
      .set('isAscending', String(query.isAscending ?? true))
      .set('pageNum', query.pageNum)
      .set('pageSize', query.pageSize);
  }

  normalizeDate(value: string | Date): Date {
    if (!value) return null as any;
    if (value instanceof Date) return value;
    return new Date(value.replace(' ', 'T'));
  }
}
