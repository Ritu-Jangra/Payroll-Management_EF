import { Component } from '@angular/core';
import { EmployeeService } from '../../core/services/Employee/employee.service';
import { IEmployee } from '../../models/Interfaces/IEmployee';
import { AsyncPipe, CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { RolesType } from '../../models/Enums/RolesType';
import { LoadingSpinnerComponent } from '../loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, AsyncPipe, LoadingSpinnerComponent],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent {
  employeeDetails$!: Observable<IEmployee>;
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
  constructor(private employeeService: EmployeeService) {}

  ngOnInit() {
      this.employeeDetails$ = this.employeeService.getMyPersonalDetails();
  }
}
