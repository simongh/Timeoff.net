import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { authGuard } from './login/auth.guard';
import { siteRoutes } from './site/site.routes';
import { SiteComponent } from './site/site.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { RegisterComponent } from './register/register.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'forgot-password',
        component: ForgotPasswordComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'reset-password',
        component: ResetPasswordComponent
    },
    {
        path: '',
        component: SiteComponent,
        canMatch: [authGuard],
        children: siteRoutes
    }
];
