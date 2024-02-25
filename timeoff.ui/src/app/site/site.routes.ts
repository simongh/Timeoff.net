import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { SettingsPageComponent } from "./settings/settings-page.component";
import { LogoutComponent } from "./logout/logout.component";
import { settingsRoutes } from "./settings/settings.routes";

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
        path: 'logout',
        component: LogoutComponent
    }
]