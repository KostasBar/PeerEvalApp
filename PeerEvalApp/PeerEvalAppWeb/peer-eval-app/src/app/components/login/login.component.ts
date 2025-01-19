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
        this.router.navigate(['working-desk']);
    },
    error: (error) =>{
        console.log('Login Error', error);
        this.invalidLogin = true;
    }
    })
  }

  // onSubmit(){
  //   console.log("Navigating to working desk");
  //   // this.router.navigate(['working-desk'])
  //   localStorage.setItem("access_token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyMUBleGFtcGxlLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJuYmYiOjE3MzcwNTcxMjAsImV4cCI6MTczNzA2NzkyMCwiaXNzIjoiaHR0cHM6Ly9jb2RpbmdmYWN0b3J5LmF1ZWIuZ3IiLCJhdWQiOiJodHRwczovL2FwaS5jb2RpbmdmYWN0b3J5LmF1ZWIuZ3IifQ.quq7g3kcjjZCquT53nUeLXPRqWRV47rfDkmW5Wn9HM8");
  //   location.reload()
  // }
  
}
