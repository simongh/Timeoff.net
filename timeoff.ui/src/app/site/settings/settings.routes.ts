import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PublicHolidaysComponent } from './public-holidays/public-holidays.component';
import { TeamsListComponent } from './teams/teams-list/teams-list.component';
import { IntegrationComponent } from './integration/integration.component';
import { TeamEditComponent } from './teams/team-edit/team-edit.component';

export default [
    {
        path: '',
        title: 'Settings',
        component: HomeComponent,
    },
    {
        path: 'public-holidays',
        title: 'Public Holidays',
        component: PublicHolidaysComponent,
    },
    {
        path: 'teams',
        title: 'Teams',
        component: TeamsListComponent,
    },
    {
        path: 'teams/:id',
        title: 'Team Details',
        component: TeamEditComponent,
    },
    {
        path: 'integration',
        title: 'Integration',
        component: IntegrationComponent,
    },
];
