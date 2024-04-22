import { Routes } from '@angular/router';
import { UserImportComponent } from './user-import/user-import.component';
import { UserCreateComponent } from './user-create/user-create.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserScheduleComponent } from './user-schedule/user-schedule.component';
import { UserCalendarComponent } from './user-calendar/user-calendar.component';
import UserAbsencesComponent from './user-absences/user-absences.component';

export const usersRoutes: Routes = [
    {
        path: '',
        component: UserListComponent,
    },
    {
        path: 'import',
        component: UserImportComponent,
    },
    {
        path: 'add',
        component: UserCreateComponent,
    },
    {
        path: ':id',
        component: UserEditComponent,
    },
    {
        path: ':id/schedule',
        component: UserScheduleComponent,
    },
    {
        path: ':id/calendar',
        component: UserCalendarComponent,
    },
    {
        path: ':id/absences',
        component: UserAbsencesComponent,
    },
];
