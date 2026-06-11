import {
  APP_INITIALIZER,
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter, withComponentInputBinding, withRouterConfig } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import {
  provideHttpClient,
  withXsrfConfiguration,
  withInterceptors,
  withFetch,
} from '@angular/common/http';
import { AuthService } from './core/services/Auth/auth.service';
import { authHeaderInterceptor } from './core/interceptors/auth-header-interceptor';
import { csrfInterceptor } from './core/interceptors/csrf-interceptor';
import { refreshInterceptor } from './core/interceptors/refresh-interceptor';
import { errorInterceptor } from './core/interceptors/error-interceptor';

export const initAuth = (authService: AuthService) => () => {
  console.log('Initializing AuthService from APP_INITIALIZER');
  return authService.initApp();
};

export const appConfig: ApplicationConfig = {
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initAuth,
      deps: [AuthService],
      multi: true,
    },
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes,
      withComponentInputBinding(),
      withRouterConfig({
        onSameUrlNavigation: 'reload',
        paramsInheritanceStrategy: 'always',
      }),
    ),
    provideClientHydration(),
    provideHttpClient(
      withFetch(),
      withInterceptors([
        authHeaderInterceptor,
        csrfInterceptor,
        refreshInterceptor,
        errorInterceptor,
      ]),
      withXsrfConfiguration({
        cookieName: 'XSRF-TOKEN',
        headerName: 'X-XSRF-TOKEN',
      }),
    ),
  ],
};
