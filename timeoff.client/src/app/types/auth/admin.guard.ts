import { inject } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from './auth.service';

export const adminGuard = () => {
    const currentUser = inject(AuthService);
    const router = inject(Router);

    if (currentUser.isAdmin()) {
        return true;
    }

    return router.createUrlTree(['/']);
};
