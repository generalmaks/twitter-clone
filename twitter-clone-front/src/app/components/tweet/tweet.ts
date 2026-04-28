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

  private readonly userService = inject(UsersService);
  private readonly likeService = inject(LikesService);

  ngOnInit(): void {
    this.userService.getById(this.post.authorId!)
      .subscribe(user => {
        this.author.set(user);
      });
    this.likeService.getPostLikesCount(this.post.id!)
      .subscribe(res =>
        this.likes.set(res)
      )
  }
}
