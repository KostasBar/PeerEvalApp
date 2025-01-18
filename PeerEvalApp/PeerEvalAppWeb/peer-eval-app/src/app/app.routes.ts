import { Routes } from '@angular/router';
import { WorkingDeskComponent } from './components/working-desk/working-desk.component';
import { UserEvaluationCyclesComponent } from './components/user-evaluation-cycles/user-evaluation-cycles.component';
import { LoginComponent } from './components/login/login.component';
import { UsersToEvaluateComponent } from './components/users-to-evaluate/users-to-evaluate.component';

export const routes: Routes = [
    {path: 'working-desk', component: WorkingDeskComponent},
    {path: 'user-evaluation-cycles', component: UserEvaluationCyclesComponent},
    {path: 'login', component: LoginComponent},
    {path: 'users-to-evaluate', component: UsersToEvaluateComponent}
];
