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
import { Router, RouterModule } from '@angular/router';
import { Mappers } from '../../utils/mappers';
import { AppComponent } from '../../app.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule],
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
  

   /** 
   * Handles the form submission for user login.
   * Validates the input and sends the login request to the authentication service.
   */
  onSubmit() {
    const credentials = this.loginForm.value as UserLogin;
    console.log(credentials)
    this.authService.login(credentials).subscribe({
      next: (response) => {

        // Get and store the retrieved token
        const access_token = response.token.token;
        localStorage.setItem("access_token", access_token);

        const decodedTokenSubject = Mappers.mapDecodedTokenToLoggedInUser(jwtDecode(access_token))
        
        this.authService.user.set({
            id: decodedTokenSubject.id,
            email: decodedTokenSubject.email,
            firstname: decodedTokenSubject.firstname,
            lastname: decodedTokenSubject.lastname,
            role: decodedTokenSubject.role
        })
        location.reload()
    },
    error: (error) =>{
        console.log('Login Error', error);
        this.invalidLogin = true;
    }
    })
  }  
}
