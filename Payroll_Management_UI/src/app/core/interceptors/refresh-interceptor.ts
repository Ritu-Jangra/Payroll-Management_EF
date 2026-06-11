import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../services/Auth/auth.service';
import { inject } from '@angular/core';

export const refreshInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  const skipUrls = [
    '/auth/login',
    '/auth/refresh',
    '/security/csrf-token'
  ];

  if (skipUrls.some(url => req.url.includes(url))) {
    console.log('RefreshInterceptor: Skipping interceptor for URL', req.url);
    return next(req);
  }

  console.log('RefreshInterceptor: Intercepting request', req.url);
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status !== 401) {
        console.log('RefreshInterceptor: Non-401 error, passing through', error);
        return throwError(() => error);
      }

      // ONLY attempt refresh if we are NOT already in bootstrap flow
      if (!authService.isAuthReady()) {
        console.log('RefreshInterceptor: Auth not ready, cannot refresh token');

        return throwError(() => error);
      }

      return authService.refreshToken().pipe(
        switchMap((res) => {
          if (!res?.accessToken) {
            console.log('RefreshInterceptor: Refresh token failed, clearing session');
            authService.clearSession();
            return throwError(() => error);
          }

          authService.setToken(res.accessToken);

          const retryReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${res.accessToken}`,
            },
          });

          console.log('RefreshInterceptor: Retrying original request with new token', retryReq);
          return next(retryReq);
        }),
        catchError((refreshError) => {
          console.log('RefreshInterceptor: Refresh token error, logging out', refreshError);
          authService.logout();
          return throwError(() => refreshError);
        })
      );
    })
  );
};
