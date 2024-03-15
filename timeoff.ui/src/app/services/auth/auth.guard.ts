import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { map } from 'rxjs';

export const authGuard = () => {
    const router = inject(Router);
    const authService = inject(AuthService);

    if (authService.isUserLoggedIn) {
        return true;
    }

    return authService.getToken().pipe(
        map(() => {
            if (authService.isUserLoggedIn) return true;

            return router.createUrlTree(['login']);
        })
    );
};
