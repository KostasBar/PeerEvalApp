import { Routes } from '@angular/router';
import { WorkingDeskComponent } from './components/working-desk/working-desk.component';
import { UserEvaluationCyclesComponent } from './components/user-evaluation-cycles/user-evaluation-cycles.component';
import { LoginComponent } from './components/login/login.component';
import { UsersToEvaluateComponent } from './components/evaluation/users-to-evaluate/users-to-evaluate.component';
import { EvaluationsDeskComponent } from './components/evaluations-desk/evaluations-desk.component';
import { Component } from '@angular/core';
import { EvaluationComponent } from './components/evaluation/evaluation.component';
import { EvaluationCyclesComponent } from './components/evaluation-cycles/evaluation-cycles.component';

export const routes: Routes = [
    {path: 'working-desk', component: WorkingDeskComponent},
    {path: 'user-evaluation-cycles', component: UserEvaluationCyclesComponent},
    {path: 'login', component: LoginComponent},
    {path: 'users-to-evaluate', component: UsersToEvaluateComponent},
    {path: 'evaluations-desk', component: EvaluationsDeskComponent},
    {path: 'evaluation', component: EvaluationComponent},
    {path: 'evaluation-cycles', component:EvaluationCyclesComponent}
];
