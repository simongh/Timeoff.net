import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { SettingsPageComponent } from "./settings/settings-page.component";
import { LogoutComponent } from "./logout/logout.component";
import { settingsRoutes } from "./settings/settings.routes";
import { TeamviewComponent } from "./teamview/teamview.component";
import { usersRoutes } from "./users/users.routes";
import { UsersPageComponent } from "./users/users-page.component";
import { ReportsPageComponent } from "./reports/reports-page.component";
import { reportsRoutes } from "./reports/reports.routes";

export const siteRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: 'settings',
        component: SettingsPageComponent,
        children : settingsRoutes
    },
    {
        path: 'users',
        component: UsersPageComponent,
        children: usersRoutes
    },
    {
        path: 'reports',
        component: ReportsPageComponent,
        children: reportsRoutes
    },
    {
        path: 'teamview',
        component: TeamviewComponent,
    },
    {
        path: 'logout',
        component: LogoutComponent
    }
]