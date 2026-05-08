import { Routes } from '@angular/router';
import { LoginPage } from './pages/login-page/login-page';
import { MainFeed } from './pages/main-feed/main-feed';
import { RegisterPage } from './pages/register-page/register-page';
import { CommentsPage } from './pages/comments-page/comments-page';

export const routes: Routes = [
    { path: '', component: MainFeed },
    { path: 'login', component: LoginPage },
    { path: 'register', component: RegisterPage},
    { path: 'tweet/:tweetId', component: CommentsPage}
];
