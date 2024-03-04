import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { PublicHolidaysComponent } from "./public-holidays/public-holidays.component";
import { TeamsListComponent } from "./teams-list/teams-list.component";
import { IntegrationComponent } from "./integration/integration.component";
import { TeamEditComponent } from "./team-edit/team-edit.component";

export const settingsRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: 'public-holidays',
        component: PublicHolidaysComponent
    },
    {
        path: 'teams',
        component: TeamsListComponent
    },
    {
        path: 'teams/:id',
        component: TeamEditComponent,
    },
    {
        path: 'integration',
        component: IntegrationComponent,
    },
]