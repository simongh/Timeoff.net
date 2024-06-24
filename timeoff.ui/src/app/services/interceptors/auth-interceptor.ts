import {
    HttpEvent,
    HttpHandlerFn,
    HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
    const currentUser = inject(LoggedInUserService);

    const authReq = req.clone({
        setHeaders: {
            Authorization: 'Bearer ' + currentUser.token(),
        },
    });

    return next(authReq);
}