import { Routes } from '@angular/router';

import { adminGuard } from '@app-types/auth/admin.guard';

import { Home } from './home/home';
import { Logout } from './logout/logout';

export const siteRoutes: Routes = [
  {
    path: '',
    title: 'Home',
    component: Home,
  },
  {
    path: 'settings',
    canActivate: [adminGuard],
    canActivateChild: [adminGuard],
    loadChildren: () => import('./settings/settings.routes'),
  },
  {
    path: 'users',
    title: 'Users',
    canActivate: [adminGuard],
    canActivateChild: [adminGuard],
    loadChildren: () => import('./users/users.routes'),
  },
  {
    path: 'logout',
    loadComponent: () => Logout,
  },
];
