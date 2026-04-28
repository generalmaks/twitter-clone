import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LikeDto } from '../models/like.models';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  private readonly url = `${environment.apiUrl}/like`;

  constructor(private http: HttpClient) {}

  getById(likeId: number): Observable<LikeDto> {
    return this.http.get<LikeDto>(`${this.url}/${likeId}`);
  }

  getByPostId(postId: number): Observable<LikeDto[]> {
    return this.http.get<LikeDto[]>(`${this.url}/post/${postId}`);
  }

  getByUserId(userId: number): Observable<LikeDto[]> {
    return this.http.get<LikeDto[]>(`${this.url}/user/${userId}`);
  }

  getPostLikesCount(postId: number): Observable<number> {
    return this.http.get<number>(`${this.url}/post/${postId}/count`);
  }

  isPostLikedByUser(postId: number, userId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}/post/${postId}/user/${userId}`);
  }

  likePost(postId: number, userId: number): Observable<LikeDto> {
    return this.http.post<LikeDto>(`${this.url}/${postId}/${userId}`, null);
  }

  unlikePost(postId: number, userId: number): Observable<LikeDto> {
    return this.http.delete<LikeDto>(`${this.url}/${postId}/${userId}`);
  }
}