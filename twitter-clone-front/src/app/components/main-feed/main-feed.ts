import { Component, inject, OnInit, signal } from '@angular/core';
import { Tweet } from "../tweet/tweet";
import { GetPost, PostService } from '../../api/http';

@Component({
  selector: 'app-main-feed',
  imports: [Tweet],
  templateUrl: './main-feed.html',
  styleUrl: './main-feed.css',
  standalone: true
})
export class MainFeed implements OnInit {
  private readonly postService = inject(PostService);

  readonly posts = signal<GetPost[] | null>(null);

  ngOnInit(): void {
    this.postService.apiV1PostsGet(1, 50).subscribe(posts => {
      this.posts.set(posts)
    })
  }
}
