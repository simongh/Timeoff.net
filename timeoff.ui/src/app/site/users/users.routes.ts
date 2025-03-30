import { UserImportComponent } from './user-import/user-import.component';
import { UserCreateComponent } from './user-create/user-create.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserScheduleComponent } from './user-schedule/user-schedule.component';
import { UserCalendarComponent } from './user-calendar/user-calendar.component';
import UserAbsencesComponent from './user-absences/user-absences.component';
import { UsersComponent } from './users.component';

export default [
    {
        path: '',
        title: 'Employees',
        component: UserListComponent,
    },
    {
        path: 'import',
        title: 'Import Employees',
        component: UserImportComponent,
    },
    {
        path: 'add',
        title: 'New Employee',
        component: UserCreateComponent,
    },
    {
        path: ':id',
        title: 'Employee Details',
        component: UsersComponent,
        children: [
            {
                path: '',
                component: UserEditComponent,
            },
            {
                path: 'schedule',
                component: UserScheduleComponent,
            },
            {
                path: 'calendar',
                component: UserCalendarComponent,
            },
            {
                path: 'absences',
                component: UserAbsencesComponent,
            },
        ],
    },
];
