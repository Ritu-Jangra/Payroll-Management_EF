import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/Auth/auth.service';
import { PermissionType } from '../../models/Enums/PermissionsType';
import { RolesType } from '../../models/Enums/RolesType';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent {
  constructor(
    public authService: AuthService,
    private router: Router,
  ) {}

  menuItems = [
    {
      // Employee-Manager-Admin shared item
      label: 'My Profile',
      route: '/dashboard/me',
      roles: [RolesType.Employee, RolesType.Manager, RolesType.Admin],
      permissions: [PermissionType.VIEW_PERSONAL_INFO],
    },
    {
      // Admin-specific items
      label: 'Add Employee',
      route: '/dashboard/employees/add',
      roles: [RolesType.Admin],
      permissions: [PermissionType.ADD_EMPLOYEE],
    },
    {
      // Admin-Manager shared item
      label: 'Team',
      route: '/dashboard/employees',
      roles: [RolesType.Admin, RolesType.Manager],
      permissions: [PermissionType.VIEW_TEAM],
    },
    {
      // Admin-specific item
      label: 'Leave Requests',
      route: '/dashboard/leaves',
      roles: [RolesType.Admin],
      permissions: [PermissionType.VIEW_TEAM_LEAVES],
    },
    {
      // Manager-specific item
      label: 'Leave Requests',
      route: '/dashboard/leaves/team',
      roles: [RolesType.Manager],
      permissions: [PermissionType.VIEW_TEAM_LEAVES],
    },
    {
      // Employee-Manager shared item
      label: 'Apply Leave',
      route: '/dashboard/leaves/apply',
      roles: [RolesType.Employee, RolesType.Manager],
      permissions: [PermissionType.APPLY_LEAVE],
    },
    {
      // Employee-Manager shared item
      label: 'My Leaves',
      route: '/dashboard/leaves/my',
      roles: [RolesType.Employee, RolesType.Manager],
      permissions: [PermissionType.VIEW_MY_LEAVES],
    },
  ];

  get filteredMenu() {
  return this.menuItems.filter(item => {
    const hasPermission = item.permissions
      ? this.authService.hasAnyPermission(item.permissions)
      : true;

    const hasRole = item.roles
      ? this.authService.hasAnyRole(item.roles)
      : true;

    return hasPermission && hasRole;
  });
}

  logout() {
    console.log('Logout initiated from DashboardComponent');

    this.authService.logout().subscribe({
      next: () => {
        console.log('Logout successful, navigating to login');
        this.router.navigate(['/login'], { replaceUrl: true });
      },
      error: (err) => {
        console.error('Logout failed', err);
        // Still navigate to login as fallback
        this.router.navigate(['/login'], { replaceUrl: true });
      },
    });
  }
}
