import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Groups, SubmitGroup } from '../interfaces/groups';

const API_URL = `${environment.apiURL}/Group`;

@Injectable({
  providedIn: 'root'
})
export class GroupsService {
  http: HttpClient = inject(HttpClient);
  
  getAllGroups(){
    const url = `${API_URL}/GetAllGroups`
    return this.http.get<Groups[]>(url);
  }

  addGroup(group: SubmitGroup){
    const url = `${API_URL}/AddGroup`
    return this.http.post(url, group)
  }
}
