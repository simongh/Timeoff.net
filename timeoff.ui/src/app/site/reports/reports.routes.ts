import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AllowanceUsageComponent } from './allowance-usage/allowance-usage.component';
import { LeaveComponent } from './leave/leave.component';

export const reportsRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: 'allowance',
        component: AllowanceUsageComponent,
    },
    {
        path: 'leave',
        component: LeaveComponent
    },
];
