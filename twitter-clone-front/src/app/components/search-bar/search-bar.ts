import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SearchService } from '../../services/search.service';


@Component({
  selector: 'app-search-bar',
  imports: [
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './search-bar.html',
  styleUrl: './search-bar.css',
})
export class SearchBar {
  readonly searchText = signal('');
  private readonly searchPostsService = inject(SearchService)

  search() {
    const query = this.searchText().trim();
    this.searchPostsService.updateQuery(query);
  }

  clearSearch() {
    this.searchText.set('');
    this.searchPostsService.updateQuery('');
  }
}
