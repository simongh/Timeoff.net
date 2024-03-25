import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LogoutComponent } from './logout/logout.component';
import { TeamviewComponent } from './teamview/teamview.component';
import { EmailAuditComponent } from './email-audit/email-audit.component';
import { FeedsComponent } from './feeds/feeds.component';

export const siteRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: 'settings',
        loadChildren: () => import('./settings/settings.routes').then((m) => m.settingsRoutes),
    },
    {
        path: 'users',
        loadChildren: () => import('./users/users.routes').then((m) => m.usersRoutes),
    },
    {
        path: 'reports',
        loadChildren: () => import('./reports/reports.routes').then((m) => m.reportsRoutes),
    },
    {
        path: 'audit/emails',
        component: EmailAuditComponent,
    },
    {
        path: 'teamview',
        component: TeamviewComponent,
    },
    {
        path: 'feeds',
        component: FeedsComponent,
    },
    {
        path: 'logout',
        component: LogoutComponent,
    },
];
