import { CanActivateFn, CanActivateChildFn, Router } from '@angular/router';
import { AuthService } from '../services/Auth/auth.service';
import { inject } from '@angular/core';

export const AuthGuard: CanActivateFn & CanActivateChildFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (!auth.isAuthReady()) {
    console.warn('AuthGuard: Authentication not ready, allowing route match to proceed', route);
    return true;
  }

  if (!auth.isAuthenticated()) {
    return router.createUrlTree(['/login']);
  }

  const roles = route.data?.['roles'];
  const permissions = route.data?.['permissions'];

  if (roles && !auth.hasAnyRole(roles)) {
    return router.createUrlTree(['/dashboard']);
  }

  if (permissions && !auth.hasAnyPermission(permissions)) {
    return router.createUrlTree(['/dashboard']);
  }

  return true;
};
