import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';

const API_URL = `${environment.apiURL}/Evaluation`;

@Injectable({
  providedIn: 'root'
})
export class EvaluationService {

  http: HttpClient = inject(HttpClient);

  submitEvaluation(newEvaluation: any){
    const url = `${API_URL}/submit`;
    return this.http.post(url, newEvaluation);
  }
  
}
 