import { Routes } from '@angular/router';

import { adminGuard } from '@services/auth/admin.guard';

import { HomeComponent } from './home/home.component';
import { LogoutComponent } from './logout/logout.component';
import { TeamviewComponent } from './teamview/teamview.component';
import { EmailAuditComponent } from './email-audit/email-audit.component';
import { FeedsComponent } from './feeds/feeds.component';
import { RequestsComponent } from './requests/requests.component';

export const siteRoutes: Routes = [
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: 'settings',
        canActivate: [adminGuard],
        loadChildren: () => import('./settings/settings.routes').then((m) => m.settingsRoutes),
    },
    {
        path: 'users',
        canActivate: [adminGuard],
        loadChildren: () => import('./users/users.routes').then((m) => m.usersRoutes),
    },
    {
        path: 'reports',
        canActivate: [adminGuard],
        loadChildren: () => import('./reports/reports.routes').then((m) => m.reportsRoutes),
    },
    {
        path: 'audit/emails',
        canActivate: [adminGuard],
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
        path: 'requests',
        component: RequestsComponent,
    },
    {
        path: 'logout',
        component: LogoutComponent,
    },
];
