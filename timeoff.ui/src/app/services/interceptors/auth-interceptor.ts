import { HttpEvent, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, switchMap } from 'rxjs';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
    const currentUser = inject(LoggedInUserService);

    if (!currentUser.isUserLoggedIn()) {
        return next(req);
    }
    
    const authReq = req.clone({
        setHeaders: {
            Authorization: 'Bearer ' + currentUser.token(),
        },
    });

    if (currentUser.needsExtending()) {
        return currentUser.extend().pipe(switchMap(() => next(authReq)));
    }

    return next(authReq);
}
