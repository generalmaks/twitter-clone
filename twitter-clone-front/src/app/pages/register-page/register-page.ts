import { Component, inject } from '@angular/core';
import { NgForm, FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButton } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { RegisterUser } from '../../api/http/models/user.models';
import { AuthService } from '../../api/http/services/auth.service';

@Component({
  selector: 'app-register-page',
  imports: [
    FormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButton,
    RouterLink
  ],
  templateUrl: './register-page.html',
  styleUrl: './register-page.css',
})
export class RegisterPage {
  model: RegisterUser = {
    username: '',
    displayUsername: '',
    email: '',
    unhashedPassword: '',
    bio: ''
  };

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  onSubmit(form: NgForm) {
    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }

    this.authService.register(this.model).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: error => {
        console.error('Registration failed', error);
      }
    });
  }
}