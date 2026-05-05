import { Component, signal } from '@angular/core';
import { Header } from './components/header/header';
import { LeftSidebar } from './components/left-sidebar/left-sidebar';
import { RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-root',
  imports: [Header, LeftSidebar, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('twitter-clone');
}
