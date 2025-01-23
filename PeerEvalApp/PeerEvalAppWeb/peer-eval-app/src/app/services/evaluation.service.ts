import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { EvalByGroup } from '../interfaces/new-evaluation';

const API_URL = `${environment.apiURL}/Evaluation`;

@Injectable({
  providedIn: 'root'
})
export class EvaluationService {

  http: HttpClient = inject(HttpClient);

  /**
   * Submiots an evaluation form from a user
   * @param newEvaluation The evaluation form
   * @returns 
   */
  submitEvaluation(newEvaluation: any){
    const url = `${API_URL}/submit`;
    return this.http.post(url, newEvaluation);
  }

  /**
   * Returns all the evaluations by group and by cycle (only for their Evaluatee)
   * @param groupId the id of the group whose evaluations are retrieved
   * @param cycleId the id of the cycle whose evaluations are retrieved
   * @returns An array of EvalByGroup
   */
  getEvaluationsByGroup(groupId: number, cycleId: number)
  {
    const url = `${API_URL}/evaluations-by-group/${groupId}/${cycleId}`
    return this.http.get<EvalByGroup[]>(url);
  }
  
}
 