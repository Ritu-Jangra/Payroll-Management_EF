import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { IEmployee } from '../../../models/Interfaces/IEmployee';
import { shareReplay } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getAllEmployees() {
    return this.http.get<IEmployee[]>(`${this.baseUrl}/v2/employee/team`).pipe(
      shareReplay(1),
    );
  }

  getMyPersonalDetails() {
    return this.http.get<IEmployee>(`${this.baseUrl}/v2/employee/me`).pipe(
      shareReplay(1),
    );
  }
}
