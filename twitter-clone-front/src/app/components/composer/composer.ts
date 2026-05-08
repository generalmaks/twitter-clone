import { Component, inject, Input, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButton } from '@angular/material/button';
import { PostsService } from '../../api/http/services/post.service';
import { CreatePost } from '../../api/http/models/post.models';

@Component({
  selector: 'app-composer',
  imports: [
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButton,
  ],
  templateUrl: './composer.html',
  styleUrl: './composer.css',
})
export class Composer {
  @Input() replyToPostId: number | null = null;
  readonly textContent = signal<string>('');

  private readonly postService = inject(PostsService);

  submitPost() {
    const text = this.textContent().trim();

    if (!text) {
      return;
    }

    const createPost: CreatePost = {
      replyToPostId: this.replyToPostId,
      textContent: text
    };

    this.postService.create(createPost).subscribe(() => {
      this.textContent.set('');
      window.location.reload()
    });
  }
}
