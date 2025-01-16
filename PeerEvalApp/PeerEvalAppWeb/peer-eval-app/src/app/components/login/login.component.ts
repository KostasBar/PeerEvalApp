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
import { Mappers } from '../../utils/mappers';

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
    console.log(credentials)
    this.authService.login(credentials).subscribe({
      next: (response) => {
        console.log('********************************************************************************************************************************************************')
        const access_token = response.token.token;
        console.log('********************************************************************************************************************************************************')
        console.log(access_token);
        localStorage.setItem("access_token", access_token);
        
        const decodedTokenSubject = Mappers.mapDecodedTokenToLoggedInUser(jwtDecode(access_token))
        console.log(decodedTokenSubject)
        
        this.authService.user.set({
            id: decodedTokenSubject.id,
            email: decodedTokenSubject.email,
            role: decodedTokenSubject.role
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
