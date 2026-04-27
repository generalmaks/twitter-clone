import { Component } from '@angular/core';
import { Tweet } from "../tweet/tweet";

@Component({
  selector: 'app-main-feed',
  imports: [Tweet],
  templateUrl: './main-feed.html',
  styleUrl: './main-feed.css',
})
export class MainFeed {}
