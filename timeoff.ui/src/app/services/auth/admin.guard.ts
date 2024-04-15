import { inject } from "@angular/core";
import { Router } from "@angular/router";

import { LoggedInUserService } from "@services/logged-in-user/logged-in-user.service";

export const adminGuard = () => {
    const currentUser = inject(LoggedInUserService);
    const router = inject(Router);

    if (currentUser.isAdmin) {
        return true;
    }

    return router.createUrlTree(['/']);
};
