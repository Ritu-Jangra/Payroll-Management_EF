import { HttpInterceptorFn } from '@angular/common/http';
import { PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export const csrfInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID);

  // Skip interceptor logic if not running in a browser environment
  if (!isPlatformBrowser(platformId)) return next(req);

  // Get CSRF token from cookies and add to headers if exists
  const csrfToken = document.cookie
    .split('; ')
    .find((c) => c.startsWith('XSRF-TOKEN='))
    ?.split('=')[1];

    console.log('CSRF Interceptor: Intercepting request', req.url);
    console.log('CSRF Interceptor: Current CSRF token', csrfToken);
  // Clone the request and add CSRF token to headers if it exists
  const modifiedReq = req.clone({
    withCredentials: true,
    ...(csrfToken && {
      setHeaders: { 'X-XSRF-TOKEN': csrfToken },
    }),
  });

  console.log('CSRF Interceptor: Modified request', modifiedReq);
  return next(modifiedReq);
};
