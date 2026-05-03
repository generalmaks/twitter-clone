import { Component, inject, OnInit, signal } from '@angular/core';
import { Tweet } from "../../components/tweet/tweet";
import { PostsService } from '../../api/http/services/post.service';
import { GetPost } from '../../api/http/models/post.models';

@Component({
  selector: 'app-main-feed',
  imports: [Tweet],
  templateUrl: './main-feed.html',
  styleUrl: './main-feed.css',
  standalone: true
})
export class MainFeed implements OnInit {
  private readonly postService = inject(PostsService);

  readonly posts = signal<GetPost[] | null>(null);

  ngOnInit(): void {
    this.postService.getAll(1, 50).subscribe(posts => {
      this.posts.set(posts)
    })
  }
}
