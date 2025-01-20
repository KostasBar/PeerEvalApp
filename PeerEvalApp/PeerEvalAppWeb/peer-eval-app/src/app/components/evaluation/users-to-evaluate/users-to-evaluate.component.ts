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
  @Output() currentUserToEvaluate = new EventEmitter<UsersToEvaluate>();
  constructor(private userService: UserService) {}

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

  onUserClick(userClicked: UsersToEvaluate) {
    this.currentUserToEvaluate.emit(userClicked as UsersToEvaluate);
  }
}
