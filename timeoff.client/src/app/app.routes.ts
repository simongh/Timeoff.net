import { Routes } from '@angular/router';

import { authGuard } from '@app-types/auth/auth.guard';

import { ForgotPassword } from './forgot-password/forgot-password';
import { Login } from './login/login';
import { Register } from './register/register';
import { ResetPassword } from './reset-password/reset-password';
import { Site } from './site/site';

export const routes: Routes = [
  {
    path: 'login',
    title: 'Login',
    loadComponent: () => Login,
  },
  {
    path: 'register',
    title: 'Register',
    loadComponent: () => Register,
  },
  {
    path: 'forgot-password',
    title: 'Forgot Password',
    loadComponent: () => ForgotPassword,
  },
  {
    path: 'reset-password',
    title: 'Reset Password',
    loadComponent: () => ResetPassword,
  },
  {
    path: '',
    canActivate: [authGuard],
    canActivateChild: [authGuard],
    loadComponent: ()=> Site,
    loadChildren: () => import('./site/site.routes').then((m) => m.siteRoutes),
  },
];
