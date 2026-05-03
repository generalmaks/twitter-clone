import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { GetUser, LoginResponse, LoginUser, RegisterUser } from '../models/user.models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly url = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  login(user: LoginUser): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.url}/login`, user);
  }

  register(user: RegisterUser): Observable<GetUser> {
    return this.http.post<GetUser>(this.url, user);
  }
}
