import { Component, EventEmitter, Output } from '@angular/core';
import { UsersToEvaluate } from '../../../interfaces/user';
import { UserService } from '../../../services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-users-to-evaluate',
  standalone: true,
  imports: [],
  templateUrl: './users-to-evaluate.component.html',
  styleUrl: './users-to-evaluate.component.css',
})
export class UsersToEvaluateComponent {
  usersToEvaluate: UsersToEvaluate[] | null = [];

  // Event emitter to send the selected user to the parent component.
  @Output() currentUserToEvaluate = new EventEmitter<UsersToEvaluate>();
  constructor(private userService: UserService) {}

  /** 
   * ngOnInit lifecycle hook, runs when the component is initialized.
   * It subscribes to the UserService's getUsersToEvaluate method to load the list of users.
   */
  ngOnInit() {
    // Subscribe to the observable returned by getPastEvaluationsOfUser
    this.userService.getUsersToEvaluate().subscribe({
      next: (users) => {
        this.usersToEvaluate = users;
      },
      error: (error) => {
        console.error('Failed to load evaluation users', error);
      },
    });
  }

  /** 
   * Handles a click event on a user.
   * Emits the selected user to the parent component through the currentUserToEvaluate event emitter.
   * @param userClicked - The user that was clicked and selected for evaluation.
   */
  onUserClick(userClicked: UsersToEvaluate) {
    this.currentUserToEvaluate.emit(userClicked as UsersToEvaluate);
  }
}
