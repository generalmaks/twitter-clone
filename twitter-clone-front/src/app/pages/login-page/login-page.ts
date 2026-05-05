import { Component, inject } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../api/http/services/auth.service';
import { LoginUser } from '../../api/http/models/user.models';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButton,
    RouterLink
  ],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {
  model: LoginUser = {
    email: '',
    password: ''
  };

  errorMessage = '';

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  onSubmit(form: NgForm) {
    this.errorMessage = '';

    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }

    this.authService.login(this.model).subscribe({
      next: response => {
        this.router.navigate(['/']);
      },
      error: error => {
        console.error('Login failed', error);

        if (error.status === 401) {
          this.errorMessage = 'Invalid email or password';
        } else {
          this.errorMessage = 'Login failed. Try again later.';
        }
      }
    });
  }
}