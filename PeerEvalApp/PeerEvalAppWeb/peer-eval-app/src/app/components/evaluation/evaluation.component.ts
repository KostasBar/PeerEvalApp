import { Component, Input } from '@angular/core';
import { UsersToEvaluate } from '../../interfaces/user';

@Component({
  selector: 'app-evaluation',
  standalone: true,
  imports: [],
  templateUrl: './evaluation.component.html',
  styleUrl: './evaluation.component.css'
})
export class EvaluationComponent {

  @Input() newUserToEvaluate: UsersToEvaluate | null = null;
  
  userToEvaluate: UsersToEvaluate = {
    firstname: 'none',
    lastname: 'none',
    email: 'none',
    id: 0
  };

  onSubmit(){
    
  }
}
