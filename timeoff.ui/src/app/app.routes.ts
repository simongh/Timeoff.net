import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { authGuard } from './services/auth/auth.guard';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { RegisterComponent } from './register/register.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent,
    },
    {
        path: 'forgot-password',
        component: ForgotPasswordComponent,
    },
    {
        path: 'register',
        component: RegisterComponent,
    },
    {
        path: 'reset-password',
        component: ResetPasswordComponent,
    },
    {
        path: '',
        canActivate: [authGuard],
        canActivateChild: [authGuard],
        loadChildren: () => import('./site/site.routes').then(m=>m.siteRoutes)
    },
];
