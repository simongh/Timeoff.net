import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandlerFn,
    HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError } from 'rxjs';

import { MessagesService } from '@components/messages/messages.service';

import { BYPASS_TOKEN } from './auth/auth.interceptor';


export function errorsInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
    const msgsSvc = inject(MessagesService);

    return next(req).pipe(
        catchError((e: HttpErrorResponse) => {
            if (e) {
                if (e.error?.errors) {
                    msgsSvc.addError(e.error.errors);
                }
                else if (!req.context.get(BYPASS_TOKEN)) {
                    msgsSvc.addError('Unable to handle request');
                }
            }

            throw e;
        })
    );
}
