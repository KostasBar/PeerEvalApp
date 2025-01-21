import { Component, inject } from '@angular/core';
import { EvaluationCycle, InitializeCycle, UpdateCycle } from '../../interfaces/evaluation-cycles';
import { Subscription } from 'rxjs';
import { EvaluationCyclesService } from '../../services/evaluation-cycles.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-evaluation-cycles',
  imports: [ReactiveFormsModule],
  standalone: true,
  templateUrl: './evaluation-cycles.component.html',
  styleUrl: './evaluation-cycles.component.css'
})
export class EvaluationCyclesComponent {
  evaluationCycles : EvaluationCycle[] | null = null;
  private subscriptions: Subscription = new Subscription();
  cycleService = inject(EvaluationCyclesService);

  cycleForm = new FormGroup({
    title: new FormControl('', Validators.required),
    duration: new FormControl('', Validators.required)
  })

  ngOnInit() {
    // Subscribe to the observable returned by getPastEvaluationsOfUser
    this.subscriptions.add(
      this.cycleService.getAllEvaluationCycles().subscribe({
        next: (cycles) => {
          this.evaluationCycles = cycles
          console.log(cycles);
          
        },
        error: (error) => {
          this.evaluationCycles = null;
          console.error('Failed to load evaluation cycles', error);
        }
      })
    );    
  }

  ngOnDestroy() {
    // Unsubscribe to prevent memory leaks
    this.subscriptions.unsubscribe();
  }

  /**
   * Handles the change event when a user selects a postpone duration.
   * @param event The change event from the select element.
   * @param cycleId The ID of the cycle to postpone.
   */
  onPostponeChange(event: Event, cycleId: number): void {
    const selectElement = event.target as HTMLSelectElement;
    const postponeValue = selectElement.value;

    if (postponeValue) {
      const postponeWeeks = parseInt(postponeValue, 10);
      const confirmation = window.confirm(`Are you sure you want to postpone cycle ${cycleId} by ${postponeWeeks} week(s)?`);

      if (confirmation) {
        this.updateCycle(cycleId, postponeWeeks);
      }

      // Reset the dropdown to its default state
      selectElement.value = "";
    }
  }

  /**
   * Updates the Evaluation cycle by postponing it for 1 or 2 weeks
   * @param cycleId The cycle to be postponed
   * @param weeks The number of weeks it is supposed to be postoponed
   * @returns 
   */
  updateCycle(cycleId: number, weeks: number){
    if (weeks != 0 && cycleId != 0){
      const updateJson: UpdateCycle = {
        id: cycleId,
        status:0, // The cycle remains open
        endWeek: weeks
      }

      this.cycleService.updateCycle(updateJson).subscribe({
        next: () => {
          location.reload()
        },
        error: (error: any) => {
          window.alert(error)
          return;
        }
      })
    }
    else{
      return
    }  
  }

  /**
   * Closes an evaluation cycle by updating its status.
   * @param cycleId The cycled to be closed.
   */
  closeCycle(cycleId: number){
    const updateJson: UpdateCycle = {
      id: cycleId,
      status:1, // Closes the cycle
      endWeek: 0 // Dont care about weeks
    }

    this.cycleService.updateCycle(updateJson).subscribe({
      next: () => {
        location.reload()
      },
      error: (error: any) => {
        console.log(error);
        
        return;
      }
    })
  }
  
  initializeCycle(value: any){
    const confirmation = window.confirm(`Are you sure you want to Iitialize a new Evaluation Cycle?`);
    console.log(value);
    
    if (confirmation) {
      const initializeJson : InitializeCycle = {
        title: value.title,
        weeks: Number(value.duration)
      }
      this.cycleService.initializeCycle(initializeJson).subscribe({
        next: () => {
          location.reload()
        },
        error: (error: any) => {
          return;
        }
      })
    }
  }
}
