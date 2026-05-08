import { Component, inject, Input, OnInit, signal } from '@angular/core';
import {
  MatCard,
  MatCardActions,
  MatCardAvatar,
  MatCardContent,
  MatCardHeader,
  MatCardSubtitle,
  MatCardTitle,
} from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { GetPost } from '../../api/http/models/post.models';
import { GetUser } from '../../api/http/models/user.models';
import { UsersService } from '../../api/http/services/user.service';
import { LikesService } from '../../api/http/services/like.service';
import { AuthService } from '../../api/http/services/auth.service';
import { PostsService } from '../../api/http/services/post.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tweet',
  imports: [
    MatCard,
    MatCardHeader,
    MatCardAvatar,
    MatCardTitle,
    MatCardSubtitle,
    MatCardContent,
    MatCardActions,
    MatIcon,
  ],
  templateUrl: './tweet.html',
  styleUrl: './tweet.css',
  standalone: true
})
export class Tweet implements OnInit {
  @Input({ required: true }) post!: GetPost;
  readonly author = signal<GetUser | null>(null);
  readonly likes = signal<number>(0);
  readonly replies = signal<number>(0);
  readonly isLiked = signal<boolean>(false);

  private readonly userService = inject(UsersService);
  private readonly likeService = inject(LikesService);
  private readonly authService = inject(AuthService);
  private readonly postService = inject(PostsService);
  private readonly router = inject(Router);

  ngOnInit(): void {
    this.userService.getById(this.post.authorId)
      .subscribe(user => {
        this.author.set(user);
      });
    this.likeService.getPostLikesCount(this.post.id)
      .subscribe(res =>
        this.likes.set(res)
      );
    this.authService.getUserInfo().subscribe(user => {
      this.likeService.isPostLikedByUser(this.post.id, user!.id).subscribe(isLiked => {
        this.isLiked.set(isLiked)
      })
    })
    this.postService.getRepliesCount(this.post.id).subscribe(count => 
      this.replies.set(count)
    )
  }

  onLike() {
    if (this.isLiked()) {
      this.likeService.unlikePost(this.post.id!).subscribe(() => {
        this.isLiked.set(false);
        this.likes.update(count => count - 1);
      });
    } else {
      this.likeService.likePost(this.post.id!).subscribe(() => {
        this.isLiked.set(true);
        this.likes.update(count => count + 1);
      });
    }
  }

  onReplies(){
    this.router.navigate(['/tweet', this.post.id])
  }
}
