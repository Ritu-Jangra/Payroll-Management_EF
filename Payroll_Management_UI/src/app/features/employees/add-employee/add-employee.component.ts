import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [],
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css'],
})
export class AddEmployeeComponent {
  
  employee = new FormGroup({
  FirstName: new FormControl<number>(100, [Validators.min(100), Validators.max(10000), Validators.required]),
  empName:new FormControl<string>('', [Validators.required]),
  salary: new FormControl<number>(1000, [Validators.required,Validators.min(1000), Validators.max(10000)])
});
}
