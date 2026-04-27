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
import { GetPost, GetUser, PostDto, PostService, UserService } from '../../api/http';

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

  private readonly userService = inject(UserService);

  ngOnInit(): void {
    this.userService.apiV1UsersIdGet(this.post.authorId!)
      .subscribe(user => {
        this.author.set(user);
      });
  }
}
