import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CreatePost, GetPost, PostDto } from '../models/post.models';

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  private readonly url = `${environment.apiUrl}/posts`;

  constructor(private http: HttpClient) {}

  getById(id: number): Observable<GetPost> {
    return this.http.get<GetPost>(`${this.url}/${id}`);
  }

  getAll(page = 1, pageSize = 20): Observable<GetPost[]> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<GetPost[]>(this.url, { params });
  }

  getRepliesCount(id: number): Observable<number> {
    return this.http.get<number>(`${this.url}/count/${id}`);
  }

  create(post: CreatePost): Observable<PostDto> {
    return this.http.post<PostDto>(this.url, post);
  }

  delete(id: number): Observable<PostDto> {
    return this.http.delete<PostDto>(`${this.url}/${id}`);
  }
}
