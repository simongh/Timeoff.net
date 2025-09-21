import { HttpContextToken, HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, switchMap } from 'rxjs';

import { AuthService } from './auth.service';

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  const currentUser = inject(AuthService);

  if (!currentUser.isUserLoggedIn() || req.context.get(BYPASS_TOKEN)) {
    return next(req);
  }

  const authReq = req.clone({
    setHeaders: {
      Authorization: 'Bearer ' + currentUser.token(),
    },
  });

  if (currentUser.needsExtending()) {
    return currentUser.extend().pipe(
      switchMap(() =>
        next(
          req.clone({
            setHeaders: {
              Authorization: 'Bearer ' + currentUser.token(),
            },
          })
        )
      )
    );
  }

  return next(
    req.clone({
      setHeaders: {
        Authorization: 'Bearer ' + currentUser.token(),
      },
    })
  );
}

export const BYPASS_TOKEN = new HttpContextToken(() => false);
