import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { map } from 'rxjs';

export const authGuard = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const router = inject(Router);
    const currentUser = inject(LoggedInUserService);

    if (currentUser.isUserLoggedIn() && !currentUser.needsExtending()) {
        return true;
    }

    return currentUser.extend().pipe(
        map(() => {
            if (currentUser.isUserLoggedIn()) {
                return true;
            }

            return router.createUrlTree(['login'], {
                queryParams: {
                    returnUrl: state.url,
                },
            });
        })
    );
};
