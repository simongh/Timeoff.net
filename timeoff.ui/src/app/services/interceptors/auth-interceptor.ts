import { HTTP_INTERCEPTORS, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { Injectable, Provider } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private readonly currentUser: LoggedInUserService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authReq = req.clone({
            setHeaders: {
                Authorization: 'Bearer ' + this.currentUser.token(),
            },
        });

        return next.handle(authReq);
    }
}

export const authInterceptorProvider: Provider = { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true };
