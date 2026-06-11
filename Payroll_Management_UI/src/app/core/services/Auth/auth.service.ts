import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  BehaviorSubject,
  switchMap,
  tap,
  map,
  catchError,
  of,
  Observable,
  firstValueFrom,
} from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../../../environments/environment';
import { IUserClaims } from '../../../models/Interfaces/IUserClaims';
import { ILoginRequest } from '../../../models/Interfaces/ILoginRequest';
import { RolesType } from '../../../models/Enums/RolesType';
import { PermissionType } from '../../../models/Enums/PermissionsType';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private accessTokenSubject = new BehaviorSubject<string | null>(null);
  private baseUrl = environment.apiUrl;
  private authReadySubject = new BehaviorSubject<boolean>(false);
  authReady$ = this.authReadySubject.asObservable();
  private userClaimsSubject = new BehaviorSubject<IUserClaims | null>(null);
  userClaims$ = this.userClaimsSubject.asObservable();

  constructor(private http: HttpClient) {}

  initApp(): Promise<void> {
    return firstValueFrom(
      this.http
        .get(`${this.baseUrl}/security/csrf-token`, {
          withCredentials: true,
        })
        .pipe(
          switchMap(() =>
            this.http
              .post<any>(`${this.baseUrl}/auth/refresh`, {}, { withCredentials: true })
              .pipe(catchError(() => of(null))),
          ),
          tap((res) => {
            if (res?.accessToken) {
              this.setToken(res.accessToken);
            }
          }),
          map(() => void 0),
          catchError(() => of(void 0)),
        ),
    ).finally(() => {
      this.authReadySubject.next(true);
    });
  }
  
  // Check if authentication initialization is complete
  isAuthReady(): boolean {
    return this.authReadySubject.value;
  }

  // Retrieve CSRF token from the server
  getCsrfToken() {
    return this.http.get<any>(`${this.baseUrl}/security/csrf-token`);
  }

  // Perform login and retrieve CSRF token
  login(loginRequest: ILoginRequest) {
    return this.http
      .post<any>(`${this.baseUrl}/auth/login`, {
        username: loginRequest.username,
        password: loginRequest.password,
      })
      .pipe(
        switchMap((loginRes) => {
          this.setToken(loginRes.accessToken);
          return this.getCsrfToken().pipe(map(() => loginRes));
        }),
      );
  }

  // Refresh the access token using the refresh token endpoint
  refreshToken() {
    return this.getCsrfToken().pipe(
      switchMap(() => this.http.post<any>(`${this.baseUrl}/auth/refresh`, {})),
      tap((res) => {
        if (res?.accessToken) {
          this.setToken(res.accessToken);
        }
      }),
    );
  }

  // Perform logout by calling the logout endpoint and clearing the token
  logout(): Observable<any> {
    return this.getCsrfToken().pipe(
      switchMap(() => this.http.post<any>(`${this.baseUrl}/auth/logout`, {})),
      tap(() => {
        this.clearSession();
        // Prevent accidental refresh reuse
        this.authReadySubject.next(true);
      }),
    );
  }

  setToken(token: string) {
    this.accessTokenSubject.next(token);

    const decoded: any = jwtDecode(token);

    console.log('Decoded JWT:', decoded);
    const claims: IUserClaims = {
      role: decoded.role || decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      permissions: Array.isArray(decoded.Permission)
        ? decoded.Permission
        : decoded.Permission
          ? [decoded.Permission]
          : [],
    };

    console.log('Mapped Claims:', claims);
    this.userClaimsSubject.next(claims);
  }

  hasRole(role: RolesType): boolean {
    return this.userClaimsSubject.value?.role === role;
  }

  hasAnyRole(roles: RolesType[]): boolean {
    const claims = this.userClaimsSubject.value;
    return claims ? roles.includes(claims.role) : false;
  }

  hasPermission(permission: PermissionType): boolean {
    return this.userClaimsSubject.value?.permissions.includes(permission) ?? false;
  }

  hasAnyPermission(perms: PermissionType[]): boolean {
    return perms.some((permission) => this.hasPermission(permission));
  }

  getToken() {
    return this.accessTokenSubject.value;
  }

  clearSession() {
    this.accessTokenSubject.next(null);
    this.userClaimsSubject.next(null);
  }

  isAuthenticated(): boolean {
    return !!this.accessTokenSubject.value && this.authReadySubject.value;
  }
}
