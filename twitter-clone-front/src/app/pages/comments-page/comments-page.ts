import { Component, inject, OnInit, signal } from '@angular/core';
import { GetPost } from '../../api/http/models/post.models';
import { ActivatedRoute } from '@angular/router';
import { PostsService } from '../../api/http/services/post.service';
import { Tweet } from "../../components/tweet/tweet";
import { MatDivider } from '@angular/material/divider'

@Component({
  selector: 'app-comments-page',
  imports: [Tweet, MatDivider],
  templateUrl: './comments-page.html',
  styleUrl: './comments-page.css',
})
export class CommentsPage implements OnInit {
  mainPost = signal<GetPost | null>(null);
  readonly replies = signal<GetPost[] | null>(null);

  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly postService = inject(PostsService)

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      const id = +params.get('tweetId')!;
      this.postService.getById(id).subscribe(post => {
        this.mainPost.set(post)
      })

      this.postService.getReplies(id).subscribe(posts => {
        this.replies.set(posts)
      })
    })
  }
}
