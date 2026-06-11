import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../core/services/Employee/employee.service';
import { IEmployee } from '../../models/Interfaces/IEmployee';
import { RolesType } from '../../models/Enums/RolesType';
import { catchError, Observable, of } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
})
export class EmployeesComponent implements OnInit {
  imagePath = (Role: RolesType) => {
    switch (Role) {
      case RolesType.Admin:
        return `${Role}.jpg`;
      case RolesType.Manager:
        return `${Role}.jpg`;
      case RolesType.Employee:
        return `${Role}.jpg`;
    }
  };
  employees$!: Observable<IEmployee[]>;

  constructor(private employeeService: EmployeeService) {}

  ngOnInit() {
    this.employees$ = this.employeeService.getAllEmployees().pipe(
      catchError((err) => {
        console.error(err);
        return of([]);
      }),
    );
  }
}
