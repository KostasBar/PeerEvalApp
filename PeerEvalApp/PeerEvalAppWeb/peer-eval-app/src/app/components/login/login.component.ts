import { Component, inject } from '@angular/core';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
} from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthServiceService } from '../../services/auth-service.service';
import { LoggedInUser, UserLogin } from '../../interfaces/user';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
 
  authService = inject(AuthServiceService);
  router = inject(Router);
  loginForm = new FormGroup({
  email: new FormControl('', [Validators.required, Validators.email]),
  password: new FormControl('', [Validators.required]),
  });

  invalidLogin = false;
  

  onSubmit() {
    const credentials = this.loginForm.value as UserLogin;
    this.authService.login(credentials).subscribe({
      next: (response) => {
        console.log('********************************************************************************************************************************************************')
        const access_token = response.access_token;
        
        console.log(access_token);
        localStorage.setItem("access_token", access_token);
        
        const decodedTokenSubject = jwtDecode(access_token)
            .sub as unknown as LoggedInUser;
        console.log(decodedTokenSubject)
        
        this.authService.user.set({
            fullname: decodedTokenSubject.fullname,
            email: decodedTokenSubject.email
        })
        // this.router.navigate(['restricted-content-example']);
    },
    error: (error) =>{
        console.log('Login Error', error);
        this.invalidLogin = true;
    }
    })
  }
}
