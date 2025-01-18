import { Component, effect, inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { PastUserEvaluationCycles } from '../../interfaces/evaluation-cycles';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-user-evaluation-cycles',
  standalone: true,
  imports: [],
  templateUrl: './user-evaluation-cycles.component.html',
  styleUrl: './user-evaluation-cycles.component.css'
})
export class UserEvaluationCyclesComponent {
  private subscriptions: Subscription = new Subscription();
  pastEvaluationCycles: PastUserEvaluationCycles[] | null = [];

  constructor(private userService: UserService) {}

  ngOnInit() {
    // Subscribe to the observable returned by getPastEvaluationsOfUser
    this.subscriptions.add(
      this.userService.getPastEvaluationsOfUser().subscribe({
        next: (cycles) => {
          this.pastEvaluationCycles = cycles
        },
        error: (error) => {
          console.error('Failed to load evaluation cycles', error);
        }
      })
    );
  }

  ngOnDestroy() {
    // Unsubscribe to prevent memory leaks
    this.subscriptions.unsubscribe();
  }
}
