import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';

const API_URL = `${environment.apiURL}/EvaluatioCycle`;

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
}
