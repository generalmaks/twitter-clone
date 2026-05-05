import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { GetUser, LoginResponse, LoginUser, RegisterUser } from '../models/user.models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly url = `${environment.apiUrl}/auth`;
  private readonly tokenKey = 'userToken';

  constructor(private http: HttpClient) {}

  login(user: LoginUser): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.url}/login`, user).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
      }),
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey)
  }

  register(user: RegisterUser): Observable<GetUser> {
    return this.http.post<GetUser>(this.url, user);
  }

  isRegistered(): boolean {
    return this.getUserToken() !== null;
  }

  getUserToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getUserInfo(): Observable<GetUser | null> {
    const token = this.getUserToken();

    if (!token) {
      return of(null);
    }

    const payload = JSON.parse(atob(token.split('.')[1]));
    const userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
      ?? payload.nameid
      ?? payload.sub
      ?? payload.userId;

    return userId
      ? this.http.get<GetUser>(`${environment.apiUrl}/users/${userId}`)
      : of(null);
  }
}
