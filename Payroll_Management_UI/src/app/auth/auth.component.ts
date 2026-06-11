import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AsyncPipe, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { ILoginRequest } from '../models/Interfaces/ILoginRequest';
import { AuthService } from '../core/services/Auth/auth.service';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [FormsModule, NgIf, AsyncPipe],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
})
export class AuthComponent {
  isLoading$ = new BehaviorSubject(false);
  error$ = new BehaviorSubject<string | null>(null);

  constructor(
    private router: Router,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    if (this.authService.isAuthenticated()) {
      console.log('Already logged in → redirecting to dashboard');
      this.router.navigate(['/dashboard']);
    }
  }

  onSubmit(formData: NgForm) {
    if (!formData.valid) {
      return;
    }
    this.error$.next(null);
    this.isLoading$.next(true);

    const loginRequest: ILoginRequest = {
      username: formData.value.userName,
      password: formData.value.password,
    };

    this.authService.login(loginRequest).subscribe({
      next: (response) => {
        this.isLoading$.next(false);
        if (response?.accessToken) {
          console.log('Login successful, navigating to dashboard');
          this.router.navigate(['/dashboard']);
        }
      },
      error: (err) => {
        console.log('Login failed, showing error message', err);
        this.error$.next(err.message);
        this.isLoading$.next(false);
      },
    });
  }
}
