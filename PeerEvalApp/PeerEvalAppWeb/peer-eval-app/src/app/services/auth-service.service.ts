import { effect, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { LoggedInUser, LoginResponse, UserLogin } from '../interfaces/user';
import { environment } from '../../environments/environment.development';
import { Router } from '@angular/router';
import { Mappers } from '../utils/mappers';

const API_URL = `${environment.apiURL}/User`;

@Injectable({
  providedIn: 'root',
})
export class AuthServiceService {
  http: HttpClient = inject(HttpClient);
  router = inject(Router);

  user = signal<LoggedInUser | null>(null);

  constructor() {
    const access_token = localStorage.getItem('access_token');
    if (access_token) {
      const decodedTokenSubject = Mappers.mapDecodedTokenToLoggedInUser(
        jwtDecode(access_token)
      );

      this.user.set({
        id: decodedTokenSubject.id,
        email: decodedTokenSubject.email,
        firstname: decodedTokenSubject.firstname,
        lastname: decodedTokenSubject.lastname,
        role: decodedTokenSubject.role,
      });
    }
    effect(() => {
      if (this.user()) {
        console.log('User logged in: ', this.user()?.email);
      } else {
        console.log('No user logged in');
      }
    });
  }

  login(credentials: UserLogin) {
    return this.http.post<LoginResponse>(`${API_URL}/login`, credentials);
  }

  logout() {
    this.user.set(null);
    localStorage.removeItem('access_token');
    this.router.navigate(['app-root']);
  }
}
