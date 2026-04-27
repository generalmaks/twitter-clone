import { Component } from '@angular/core';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MatIcon } from '@angular/material/icon';


@Component({
  selector: 'app-left-sidebar',
  imports: [MatSidenavModule, MatIcon],
  templateUrl: './left-sidebar.html',
  styleUrl: './left-sidebar.css',
})
export class LeftSidebar {}
