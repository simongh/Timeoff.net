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
        title: 'Home',
        component: HomeComponent,
    },
    {
        path: 'settings',
        canActivate: [adminGuard],
        loadChildren: () => import('./settings/settings.routes'),
    },
    {
        path: 'users',
        canActivate: [adminGuard],
        loadChildren: () => import('./users/users.routes'),
    },
    {
        path: 'reports',
        canActivate: [adminGuard],
        loadChildren: () => import('./reports/reports.routes'),
    },
    {
        path: 'audit/emails',
        title: 'Email Audit',
        canActivate: [adminGuard],
        component: EmailAuditComponent,
    },
    {
        path: 'teamview',
        title: 'Team View',
        component: TeamviewComponent,
    },
    {
        path: 'feeds',
        title: 'My Feeds',
        component: FeedsComponent,
    },
    {
        path: 'requests',
        title: 'Requests',
        component: RequestsComponent,
    },
    {
        path: 'logout',
        component: LogoutComponent,
    },
];
