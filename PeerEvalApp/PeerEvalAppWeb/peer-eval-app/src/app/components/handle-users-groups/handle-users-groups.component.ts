import { Component, inject } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Groups, SubmitGroup } from '../../interfaces/groups';
import { SubmitUser, UpdateUser, UsersToEvaluate } from '../../interfaces/user';
import { UserService } from '../../services/user.service';
import { GroupsService } from '../../services/groups.service';
import { CommonModule } from '@angular/common';

const pattern: string = '^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[\\W_]).{8,}$';

@Component({
  selector: 'app-handle-users-groups',
  imports: [ReactiveFormsModule, CommonModule],
  standalone: true,
  templateUrl: './handle-users-groups.component.html',
  styleUrl: './handle-users-groups.component.css',
})
export class HandleUsersGroupsComponent {
  // Services
  userSevice = inject(UserService);
  groupsService = inject(GroupsService);

  // Used For UI
  groups: Groups[] | null = null;
  users: UsersToEvaluate[] | null = null;
  userError: string = '';

  // Forms
  newUserForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.pattern(pattern), Validators.required]),
    groupId: new FormControl('', Validators.required),
    role: new FormControl('', Validators.required),
  });

  newGroupsForm = new FormGroup({
    groupName: new FormControl('', Validators.required),
  });

  usersByGroup = new FormGroup({
    groupId: new FormControl('', Validators.required),
  });

  updatedUserForm= new FormGroup({
    id: new FormControl('', Validators.required),
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.pattern(pattern)),
    groupId: new FormControl(''),
    role: new FormControl('')
  })

  //Methods
  
  ngOnInit() {
    // Call to get Groups for dropdown
    this.groupsService.getAllGroups().subscribe({
      next: (group) => {
        this.groups = group;
      },
      error: (error) => {
        return;
      },
    });
  }

  /**
   * Makes a call that adds a New User, after constructing a new SubmitUser object.
   * @param value The form value of newUserForm
   */
  signUpUser(value: any) {
    const user: SubmitUser = {
      FirstName: value.firstName,
      LastName: value.lastName,
      Email: value.email,
      Password: value.password,
      GroupId: Number(value.groupId),
      UserRole: value.role,
    };

    this.userSevice.signUpUser(user).subscribe({
      next: () => {
        alert(`User with email ${value.email} successfully submitted!`);
      },
      error: (error) => {
        alert('Error while submiting new user!');
      },
    });
  }

  /**
   * Makes a call that adds a new Group after creating a new SubmitGroup Object.
   * @param value The form value NewGroupsForm
   */
  addGroup(value: any) {
    const submitGroup: SubmitGroup = {
      groupName: value.groupName,
    };

    this.groupsService.addGroup(submitGroup).subscribe({
      next: () => {
        location.reload();
      },
      error: (error) => {
        alert('Something went wrong while trying to add a new Group');
      },
    });
  }

  /**
   * Retrieves an Array of UsersToEvaluate Objects to sho to the Users table
   * @param value The value of usersByGroup form
   */
  retrieveUsers(value: any) {
    const idOfGroup = value.groupId;
    this.userSevice.getUsersByGroup(idOfGroup).subscribe({
      next: (usersRetrieved: any) => {
        this.users = usersRetrieved;
        this.newUserForm.reset();
      },
      error: (error) => {
        this.userError = 'An Error Occured while retrieving the users';
      },
    });
  }

  /**
   * Updates a user by sending their new data to the API.
   * The data that can be affected are the Password, the Role and/or the Group
   * @param newid The id of the user to be updated.
   * @param newfirstname The firstname of the user to be updated.
   * @param newlastname The lastname of the user to be updated.
   * @param newemail The email of the user to be updated.
   * @param newpassword The password of the user to be updated. If it is not to be updated it is empty string.
   * @param newgroup The group of the user to be updated. If it is not to be updated it is 0.
   * @param newrole The role of the user to be updated. If it is not to be updated it is the string "0".
   */
  updateUser(
    newid: number ,
    newfirstname: string ,
    newlastname: string ,
    newemail: string ,
    newpassword: string ,
    newgroup: string ,
    newrole: string 
  ) {
    const updatedUser: UpdateUser = {
      firstname: newfirstname,
      lastname: newlastname,
      email: newemail,
      role: newrole,
      password: newpassword,
      groupId: Number(newgroup),
      id: newid,
    };
    this.userSevice.updateUser(updatedUser).subscribe({
      next: ()=>{
        return;
      },
      error: ()=>{
        alert('Error while updating user!')
      }
    })
    this.retrieveUsers(this.usersByGroup.value)
  }
}
