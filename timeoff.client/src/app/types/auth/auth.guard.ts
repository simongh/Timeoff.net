import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { map } from 'rxjs';

import { AuthService } from './auth.service';

export const authGuard = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const router = inject(Router);
  const currentUser = inject(AuthService);

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
