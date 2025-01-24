import { HttpClient } from '@angular/common/http';
import { effect, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { LoggedInUser, SubmitUser, UsersToEvaluate } from '../interfaces/user';
import { Mappers } from '../utils/mappers';
import { jwtDecode } from 'jwt-decode';
import { PastUserEvaluationCycles } from '../interfaces/evaluation-cycles';
import { environment } from '../../environments/environment.development';
import { BehaviorSubject } from 'rxjs';

const API_URL = `${environment.apiURL}/User`;

@Injectable({
  providedIn: 'root',
})
export class UserService {
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

  /**
   * After checking for the user's id using the access_token,
   * calls the API at the endpoint responsible for returning a list of past evaluation
   * cycles with the results for the current  app-user
   * @returns A list of PastUserEvaluationCycles
   */
  getPastEvaluationsOfUser() {
    const user = this.user(); // Retrieve the current user from the signal
    if (!user) {
      console.error('No user logged in');
      throw new Error('No user logged in');
    }

    const userId = user.id; 
    const url = `${API_URL}/evaluations-for-user/${userId}`; // Construct the URL with user ID

    return this.http.get<PastUserEvaluationCycles[]>(url, {
      headers: {
        Accept: 'application/json',
      },
    });
  }

   /**
   * After checking for the user's id using the access_token,
   * calls the API at the endpoint responsible for returning a list of users
   * for the current app user to evaluate.
   * @returns A list of UsersToEvaluate-like structures
   */
  getUsersToEvaluate(){
    const user = this.user(); 
    if (!user) {
      console.error('No user logged in');
      throw new Error('No user logged in');
    }

    const userId = user.id; 
    const url = `${API_URL}/user-to-evaluate/${userId}`;

    return this.http.get<UsersToEvaluate[]>(url, {
      headers: {
        Accept: 'application/json',
      },
    });
  }

  signUpUser(user: SubmitUser){
    const url = `${API_URL}/register`;
    return this.http.post(url, user);
  }

  getUsersByGroup(groupId: number){
    const url = `${API_URL}/users-by-group/${groupId}`;
    return this.http.get<UsersToEvaluate[]>(url);
  }
}
