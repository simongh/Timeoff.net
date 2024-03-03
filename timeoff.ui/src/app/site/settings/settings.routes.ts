import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { PublicHolidaysComponent } from "./public-holidays/public-holidays.component";
import { TeamsComponent } from "./teams/teams.component";
import { IntegrationComponent } from "./integration/integration.component";

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
        component: TeamsComponent
    },
    {
        path: 'integration',
        component: IntegrationComponent,
    },
]