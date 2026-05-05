import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { GetUser, UpdateUser } from '../models/user.models';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly url = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  getById(id: number): Observable<GetUser> {
    return this.http.get<GetUser>(`${this.url}/${id}`);
  }

  getAll(page = 1, pageSize = 20): Observable<GetUser[]> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<GetUser[]>(this.url, { params });
  }

  update(user: UpdateUser): Observable<GetUser> {
    return this.http.patch<GetUser>(this.url, user);
  }
}
