import { ApplicationConfig } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { errorsInterceptor } from '@services/interceptors/errors-interceptor';
import { authInterceptor } from '@services/interceptors/auth-interceptor';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
    providers: [
        provideRouter(routes, withComponentInputBinding()),
        provideHttpClient(withInterceptors(
            [errorsInterceptor, authInterceptor]
        ))
    ],
};
