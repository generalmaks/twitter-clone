import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { GetPost, PostDto, UpdatePost } from '../models/post.models';

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

  create(post: PostDto): Observable<PostDto> {
    return this.http.post<PostDto>(this.url, post);
  }

  update(post: UpdatePost): Observable<PostDto> {
    return this.http.patch<PostDto>(this.url, post);
  }

  delete(id: number): Observable<PostDto> {
    return this.http.delete<PostDto>(`${this.url}/${id}`);
  }
}