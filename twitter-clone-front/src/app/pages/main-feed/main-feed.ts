import { Component, inject, OnInit, signal, OnDestroy } from '@angular/core';
import { Tweet } from "../../components/tweet/tweet";
import { PostsService } from '../../api/http/services/post.service';
import { GetPost } from '../../api/http/models/post.models';
import { Composer } from '../../components/composer/composer';
import { SearchService } from '../../services/search.service';
import { Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-main-feed',
  imports: [Tweet, Composer],
  templateUrl: './main-feed.html',
  styleUrl: './main-feed.css',
  standalone: true,
})
export class MainFeed implements OnInit, OnDestroy {
  private readonly postService = inject(PostsService);
  private readonly searchService = inject(SearchService);

  private searchSubscription?: Subscription;

  readonly posts = signal<GetPost[] | null>(null);

  ngOnInit(): void {
    this.searchSubscription = this.searchService.searchQuery$
      .pipe(
        switchMap((query) => {
          if (query.trim()) {
            return this.postService.getBySearchText(query, 1, 50);
          } else {
            return this.postService.getAll(1, 50);
          }
        })
      )
      .subscribe((posts) => {
        this.posts.set(posts);
      });
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
  }
}
