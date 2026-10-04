import { Routes } from '@angular/router';

import { Home } from './home/home';
import { PublicHolidays } from './public-holidays/public-holidays';
import { List as ListUsers } from './teams/list/list';
import { Edit as EditUsers } from './teams/edit/edit';

export default [
  {
    path: '',
    title: 'Settings',
    component: Home,
  },
  {
    path: 'public-holidays',
    title: 'Public Holidays',
    loadComponent: () => PublicHolidays,
  },
  {
    path: 'teams',
    title: 'Teams',
    loadComponent: () => ListUsers,
  },
  {
    path: 'teams/:id',
    title: 'Team Details',
    loadComponent: () => EditUsers,
  },
] as Routes;
