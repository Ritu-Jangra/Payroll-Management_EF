import { Route } from '@angular/router';
import { DashboardComponent } from '../layout/dashboard/dashboard.component';
import { AuthGuard } from '../core/guards/auth-guard';
import { PermissionType } from '../models/Enums/PermissionsType';
import { RolesType } from '../models/Enums/RolesType';

export const DASHBOARD_ROUTES: Route[] = [
  {
    path: '',
    component: DashboardComponent,
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        data: {
          roles: [RolesType.Admin, RolesType.Manager, RolesType.Employee],
        },
        loadComponent: () =>
          import('../layout/dashboard/welcome/welcome.component').then((m) => m.WelcomeComponent),
      },
      {
        path: 'me',
        data: {
          roles: [RolesType.Admin, RolesType.Manager, RolesType.Employee],
          permissions: [PermissionType.VIEW_PERSONAL_INFO],
        },
        loadComponent: () =>
          import('../shared/profile/profile.component').then((m) => m.ProfileComponent),
      },

      {
        path: 'employees',
        children: [
          {
            path: '',
            redirectTo: 'team',
            pathMatch: 'full',
          },
          {
            path: 'team',
            data: {
              roles: [RolesType.Admin, RolesType.Manager],
              permissions: [PermissionType.VIEW_TEAM],
            },
            loadComponent: () =>
              import('../features/employees/employees.component').then((m) => m.EmployeesComponent),
          },
          {
            path: 'add',
            data: {
              roles: [RolesType.Admin],
              permissions: [PermissionType.ADD_EMPLOYEE],
            },
            loadComponent: () =>
              import('../features/employees/add-employee/add-employee.component').then(
                (m) => m.AddEmployeeComponent,
              ),
          },
        ],
      },
      {
        path: 'leaves',
        children: [
          {
            path: '',
            redirectTo: 'team',
            pathMatch: 'full',
          },
          {
            path: 'team',
            data: {
              roles: [RolesType.Admin, RolesType.Manager],
              permissions: [PermissionType.VIEW_TEAM_LEAVES],
            },
            loadComponent: () =>
              import('../features/leaves/leave-requests/leave-requests.component').then(
                (m) => m.LeaveRequestsComponent,
              ),
          },
          {
            path: 'apply',
            data: {
              roles: [RolesType.Employee, RolesType.Manager],
              permissions: [PermissionType.APPLY_LEAVE],
            },
            loadComponent: () =>
              import('../features/leaves/apply-leave/apply-leave.component').then(
                (m) => m.ApplyLeaveComponent,
              ),
          },
          {
            path: 'my',
            data: {
              roles: [RolesType.Employee, RolesType.Manager],
              permissions: [PermissionType.VIEW_MY_LEAVES],
            },
            loadComponent: () =>
              import('../features/leaves/leaves.component').then((m) => m.LeavesComponent),
          },
        ],
      },
    ],
  },
];
