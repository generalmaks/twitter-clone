import { Component, inject, OnInit, signal } from '@angular/core';
import { MatToolbar } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { SearchBar } from '../search-bar/search-bar';
import { MatButton } from '@angular/material/button';
import { AuthService } from '../../api/http/services/auth.service';
import { GetUser } from '../../api/http/models/user.models';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-header',
  imports: [
    MatToolbar,
    MatIconModule,
    SearchBar,
    MatButton,
    RouterLink
],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header implements OnInit {
  private readonly authService = inject(AuthService)
  readonly userInfo = signal<GetUser | null>(null)

  ngOnInit(): void {
    this.getUserInfo()
  }

  isRegistered() {
    return this.authService.isRegistered()
  }

  getUserInfo() {
    console.log('Logged in')
    this.authService.getUserInfo().subscribe(user =>
      this.userInfo.set(user)
    )
  }
 }
