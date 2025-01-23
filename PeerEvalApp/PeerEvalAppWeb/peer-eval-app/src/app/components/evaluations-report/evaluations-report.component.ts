import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EvalByGroup } from '../../interfaces/new-evaluation';
import { EvaluationService } from '../../services/evaluation.service';
import { Groups } from '../../interfaces/groups';


@Component({
  selector: 'app-evaluations-report',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './evaluations-report.component.html',
  styleUrl: './evaluations-report.component.css'
})
export class EvaluationsReportComponent {

  evaluations: EvalByGroup[] | null = null;
  groups: Groups[] | null = null;
  evaluationService = inject(EvaluationService)
  evalByGroup  = new FormGroup({
    groupId = new FormControl('', Validators.required),
    cycleId = new FormControl('', Validators.required)
  })

  ngOnInit(){
    
  }
  getEvaluations(cycleId: number, groubId: number){
    this.evaluationService.getEvaluationsByGroup(groubId, cycleId).subscribe({
      next: (newEvalByGroup: EvalByGroup[]) => {
        this.evaluations = newEvalByGroup;
      },
      error: (error: any) => {
        window.alert('An error ocurred while trying to retrive the Evaluations')
        return;
      }
    })
  }
}
