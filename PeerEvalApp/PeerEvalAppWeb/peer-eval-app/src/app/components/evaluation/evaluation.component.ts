import { Component, inject, Input } from '@angular/core';
import { LoggedInUser, UsersToEvaluate } from '../../interfaces/user';
import {
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Mappers } from '../../utils/mappers';
import { jwtDecode } from 'jwt-decode';
import { EvaluationService } from '../../services/evaluation.service';

@Component({
  selector: 'app-evaluation',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './evaluation.component.html',
  styleUrl: './evaluation.component.css',
})
export class EvaluationComponent {
  @Input() newUserToEvaluate: UsersToEvaluate | null = null;
  @Input() cycleId: number | null = null;
  
  evaluationService: EvaluationService = inject(EvaluationService);

  evaluationForm = new FormGroup({
    selfConfidence: new FormControl('', Validators.required),
    dedication: new FormControl('', Validators.required),
    jobKnowledge: new FormControl('', Validators.required),
    qaOfWork: new FormControl('', Validators.required),
    abilityDeadlines: new FormControl('', Validators.required),
    independence: new FormControl('', Validators.required),
    commitment: new FormControl('', Validators.required),
    detailAttention: new FormControl('', Validators.required),
    workWithOthers: new FormControl('', Validators.required),
    communicationSkills: new FormControl('', Validators.required),
    underPressure: new FormControl('', Validators.required),
  });

  onSubmit(value: any) {
    const answers = Mappers.mapToJsonSubmitEvaluation(value);
    const token = localStorage.getItem('access_token');
    let user: LoggedInUser | null = null;
    if (token) {
      user = Mappers.mapDecodedTokenToLoggedInUser(jwtDecode(token));
    }

    if (!this.newUserToEvaluate?.id || !this.cycleId || !user) {
      console.log('No User');
      
      return;
    }
    const newEvaluation = Mappers.mapToNewEvaluation(
      answers,
      Number(user.id),
      this.newUserToEvaluate?.id,
      this.cycleId
    );
    console.log(newEvaluation);
    
    this.evaluationService.submitEvaluation(newEvaluation).subscribe({
      next: () => {
        this.newUserToEvaluate = null;
        //location.reload()
      },
      error: (error: any) => {
        window.alert('Error while saving the form!');
        navigator.clipboard.writeText(JSON.stringify(newEvaluation))
        return;
      }
    });
  }
}
