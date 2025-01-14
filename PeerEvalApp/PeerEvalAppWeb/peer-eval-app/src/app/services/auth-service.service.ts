import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  private endpoint = '';

  constructor() { 
    const access_token = localStorage.getItem("access_token")
        if (access_token){
            const decodedTokenSubject = jwtDecode(access_token)
                .sub as unknown as LoggedInUser
            
                this.user.set({
                    fullname: decodedTokenSubject.fullname,
                    email:decodedTokenSubject.email
                })
   }

  login(username: string | undefined, password: string | undefined): Observable<any>{
    return this.http.post<any>(this.endpoint, {username, password})
  }
}
