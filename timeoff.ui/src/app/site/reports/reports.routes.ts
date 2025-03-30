import { HomeComponent } from './home/home.component';
import { AllowanceUsageComponent } from './allowance-usage/allowance-usage.component';
import { LeaveComponent } from './leave/leave.component';

export default [
    {
        path: '',
        title: 'Reports',
        component: HomeComponent,
    },
    {
        path: 'allowance',
        title: 'Allowance Usage Report',
        component: AllowanceUsageComponent,
    },
    {
        path: 'leave',
        title: 'Leave Summary Report',
        component: LeaveComponent
    },
];
