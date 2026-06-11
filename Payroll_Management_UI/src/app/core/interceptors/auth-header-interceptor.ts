import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/Auth/auth.service';
import { PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export const authHeaderInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const platformId = inject(PLATFORM_ID);

  // Skip interceptor logic if not running in a browser environment
  if (!isPlatformBrowser(platformId)) return next(req);

  // Get token from AuthService and add to headers if exists
  const token = authService.getToken();

  console.log('AuthHeaderInterceptor: Intercepting request', req.url);
  console.log('AuthHeaderInterceptor: Current token', token);
  // Clone the request and set withCredentials to true for cross-origin requests
  const modifiedReq = req.clone({
    withCredentials: true,
    ...(token && {
      setHeaders: { Authorization: `Bearer ${token}` },
    }),
  });

  console.log('AuthHeaderInterceptor: Modified request', modifiedReq);
  return next(modifiedReq);
};
