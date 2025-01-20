import { Component } from '@angular/core';
import { UserEvaluationCyclesComponent } from '../user-evaluation-cycles/user-evaluation-cycles.component';
import { EvaluationComponent } from '../evaluation/evaluation.component';
import { Subscription } from 'rxjs';
import { EvaluationCyclesService } from '../../services/evaluation-cycles.service';
import { UsersToEvaluateComponent } from '../evaluation/users-to-evaluate/users-to-evaluate.component';
import { UsersToEvaluate } from '../../interfaces/user';

@Component({
  selector: 'app-evaluations-desk',
  standalone: true,
  imports: [UsersToEvaluateComponent, EvaluationComponent],
  templateUrl: './evaluations-desk.component.html',
  styleUrl: './evaluations-desk.component.css',
})
export class EvaluationsDeskComponent {
  private subscriptions: Subscription = new Subscription();
  existingCycleId: number | null = null;
  newUserClicked: UsersToEvaluate | null = null;
  constructor(private cycleService: EvaluationCyclesService) {}

  ngOnInit() {
    this.subscriptions.add(
      this.cycleService.getOpenEvaluationCycles().subscribe({
        next: (cycleId) => {
          this.existingCycleId = cycleId;
        },
        error: (error) => {
          this.existingCycleId = 0;
        },
      })
    );
  }

  ngOnDestroy() {
    // Unsubscribe to prevent memory leaks
    this.subscriptions.unsubscribe();
  }

    onUserClick(userClicked: UsersToEvaluate) {
      console.log(userClicked);
      this.newUserClicked = userClicked;
    }
}
