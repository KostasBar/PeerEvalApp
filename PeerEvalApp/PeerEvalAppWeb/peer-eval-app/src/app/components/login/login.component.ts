import { Component, inject } from '@angular/core';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
} from '@angular/forms';
// import { HttpClient } from '@angular/common/http';
import { AuthServiceService } from '../../services/auth-service.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule,],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
 
  authService = inject(AuthServiceService)
  loginForm = new FormGroup({
  username: new FormControl('', [Validators.required, Validators.email]),
  password: new FormControl('', [Validators.required]),
  });
  

  onSubmit(): void {
  //   // const credentials = this.loginForm.value as Credentials;

  //   this.authService
  //     .login(this.loginForm.value.username?.toString(), this.loginForm.value.password?.toString())
  //     .subscribe({
  //       next: (response) => console.log('Login successful', response),
  //       error: (error) => console.error('Login failed', error),
  //     });
  //   // }
  }
}
