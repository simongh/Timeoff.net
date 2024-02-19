import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { SettingsPageComponent } from "./settings/settings-page.component";
import { LogoutComponent } from "./logout/logout.component";

export const siteRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: 'settings',
        component: SettingsPageComponent,
    },
    {
        path: 'logout',
        component: LogoutComponent
    }
]