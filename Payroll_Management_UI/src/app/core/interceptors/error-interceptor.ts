import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const problem = error.error;

      let validationMessage = '';

      if (problem?.errors) {
        validationMessage = Object.keys(problem.errors)
          .map((key) => problem.errors[key].join(' '))
          .join(' ');
      }

      const normalizedError = {
        status: error.status,
        title: problem?.title || 'Error',
        message: problem?.detail || validationMessage || problem?.message || 'Something went wrong',
      };

      return throwError(() => normalizedError);
    }),
  );
};
