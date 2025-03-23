import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

interface AuthResponse {
  token: string;
  user: {
    id: number;
    email: string;
    roles: string[];
  };
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = environment.apiUrl || 'http://localhost:5000';
  private tokenKey = 'yes_location_token';
  private userKey = 'yes_location_user';

  private _isAuthenticated = new BehaviorSubject<boolean>(this.hasValidToken());
  private _currentUser = new BehaviorSubject<any>(this.getStoredUser());

  isAuthenticated$: Observable<boolean> = this._isAuthenticated.asObservable();
  currentUser$: Observable<any> = this._currentUser.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string): Observable<boolean> {
    return this.http
      .post<AuthResponse>(`${this.baseUrl}/api/auth/login`, { email, password })
      .pipe(
        tap((response) => {
          this.storeToken(response.token);
          this.storeUser(response.user);
          this._isAuthenticated.next(true);
          this._currentUser.next(response.user);
        }),
        map(() => true),
        catchError((error) => {
          console.error("Erreur d'authentification", error);
          return of(false);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this._isAuthenticated.next(false);
    this._currentUser.next(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isInRole(role: string): boolean {
    const user = this.getStoredUser();
    return user?.roles?.includes(role) || false;
  }

  private storeToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  private storeUser(user: any): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  private getStoredUser(): any {
    const userData = localStorage.getItem(this.userKey);
    return userData ? JSON.parse(userData) : null;
  }

  private hasValidToken(): boolean {
    const token = this.getToken();
    if (!token) return false;

    // Simple validation - in a real app, you'd check expiration
    return true;
  }
}
