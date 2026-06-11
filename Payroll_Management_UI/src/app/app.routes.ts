import { Route } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { AuthGuard } from './core/guards/auth-guard';

export const routes: Route[] = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: AuthComponent,
  },
  {
    path: 'dashboard',
    canActivate: [AuthGuard],
    loadChildren: () => import('./routes/dashboard.routes').then((m) => m.DASHBOARD_ROUTES),
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
