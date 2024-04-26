import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { routes } from './app.routes';
import { authInterceptorProvider } from '@services/interceptors/auth-interceptor';
import { errorsInterceptorProvider } from '@services/interceptors/errors-interceptor';

export const appConfig: ApplicationConfig = {
    providers: [
        provideRouter(routes, withComponentInputBinding()),
        importProvidersFrom(HttpClientModule),
        authInterceptorProvider,
        errorsInterceptorProvider,
    ],
};
