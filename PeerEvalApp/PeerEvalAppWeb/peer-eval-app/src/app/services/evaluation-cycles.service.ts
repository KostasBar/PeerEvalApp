import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';
import { EvaluationCycle, InitializeCycle, UpdateCycle } from '../interfaces/evaluation-cycles';

const API_URL = `${environment.apiURL}/EvaluationCycle`;

@Injectable({
  providedIn: 'root',
})
export class EvaluationCyclesService {
  http: HttpClient = inject(HttpClient);
  router = inject(Router);

  cycle = signal<number | null>(null);

  /**
   * Searches for an open evaluation cycle.
   * @returns The Evaluation Cycle Id if exists or 0 if no open evaluation cycle exists
   */
  getOpenEvaluationCycles(){
    const url = `${API_URL}/evaluationCycleExists`;
    return this.http.get<number>(url)
  }

  /**
   * A call that returns all the evaluation cycles
   * @returns All the evaluation cycles()
   */
  getAllEvaluationCycles(){
    const url = `${API_URL}/get-all-cycles`;
    return this.http.get<EvaluationCycle[]>(url);
  }

  updateCycle(updateJson: UpdateCycle){
    const url = `${API_URL}/updatecycle`;
    return this.http.post(url, updateJson);
  }

  initializeCycle(initializeJson: InitializeCycle){
    const url = `${API_URL}/newcycle`;
    return this.http.post(url, initializeJson);
  }


}
