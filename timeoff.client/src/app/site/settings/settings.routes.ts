import { Routes } from '@angular/router';

import { Home } from './home/home';
import { PublicHolidays } from './public-holidays/public-holidays';

export default [
    {
        path: '',
        title: 'Settings',
        component: Home
    },
    {
        path: 'public-holidays',
        title: 'Public Holidays',
        loadComponent: ()=> PublicHolidays
    }
] as Routes;