import { Component } from '@angular/core';
import { MatToolbar } from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';

@Component({
  selector: 'app-header',
  imports: [MatToolbar, MatIconModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header { }
