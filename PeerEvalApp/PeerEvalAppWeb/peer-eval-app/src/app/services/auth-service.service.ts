import { effect, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { LoggedInUser, UserLogin } from '../interfaces/user';
import { environment } from '../../environments/environment.development';
import { Router } from '@angular/router';

const API_URL=`${environment.apiURL}/user`

@Injectable({
  providedIn: 'root',
})
export class AuthServiceService {
  http: HttpClient = inject(HttpClient)
  router = inject(Router)

  user = signal<LoggedInUser | null>(null);

  constructor() {
    const access_token = localStorage.getItem('access_token');
    if (access_token) {
      const decodedTokenSubject = jwtDecode(access_token)
        .sub as unknown as LoggedInUser;

      this.user.set({
        fullname: decodedTokenSubject.fullname,
        email: decodedTokenSubject.email,
      });
    }
    effect(() => {
      if (this.user()) {
        console.log('User logged in: ', this.user()?.fullname);
      } else {
        console.log('No user logged in');
      }
    });
  }

  login(credentials: UserLogin){
    return this.http.post<{access_token: string}>(`${API_URL}/login`,credentials)

}
}
