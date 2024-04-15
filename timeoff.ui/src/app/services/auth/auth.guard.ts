import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

export const authGuard = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const router = inject(Router);
    const currentUser = inject(LoggedInUserService);

    if (currentUser.isUserLoggedIn) {
        return true;
    }

    return router.createUrlTree(['login'], {
        queryParams: {
            returnUrl: state.url,
        },
    });
};
