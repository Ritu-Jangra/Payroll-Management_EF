import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { AuthService } from '../../core/services/Auth/auth.service';
@Component({
  selector: 'app-dashboard.component',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent {
  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  ngOnInit() {
  }
}
