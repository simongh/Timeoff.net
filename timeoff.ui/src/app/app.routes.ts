import { Routes } from '@angular/router';

import { authGuard } from '@services/auth/auth.guard';

import { LoginComponent } from './login/login.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { RegisterComponent } from './register/register.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

export const routes: Routes = [
    {
        path: 'login',
        title: 'Login',
        component: LoginComponent,
    },
    {
        path: 'forgot-password',
        title: 'Forgot Password',
        component: ForgotPasswordComponent,
    },
    {
        path: 'register',
        title: 'Register',
        component: RegisterComponent,
    },
    {
        path: 'reset-password',
        title: 'Reset Password',
        component: ResetPasswordComponent,
    },
    {
        path: '',
        canActivate: [authGuard],
        canActivateChild: [authGuard],
        loadChildren: () => import('./site/site.routes').then(m=>m.siteRoutes)
    },
];
