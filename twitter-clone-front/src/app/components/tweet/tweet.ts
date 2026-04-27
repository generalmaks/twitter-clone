import { Component } from '@angular/core';
import { MatCard, MatCardHeader, MatCardAvatar, MatCardTitle, MatCardSubtitle, MatCardContent, MatCardActions, MatCardImage } from '@angular/material/card';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-tweet',
  imports: [MatCard, MatCardHeader, MatCardAvatar, MatCardTitle, MatCardSubtitle, MatCardContent, MatCardActions, MatIcon],
  templateUrl: './tweet.html',
  styleUrl: './tweet.css',
})
export class Tweet {}
