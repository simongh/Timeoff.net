import {
    HTTP_INTERCEPTORS,
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
} from '@angular/common/http';
import { Injectable, Provider } from '@angular/core';
import { MessagesService } from '@services/messages/messages.service';
import { Observable, catchError } from 'rxjs';

@Injectable()
export class ErrorsInterceptor implements HttpInterceptor {
    constructor(private readonly msgsSvc: MessagesService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError((e: HttpErrorResponse) => {
                if (e) {
                    if (e.error.errors) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to handle request');
                    }
                }

                throw e;
            })
        );
    }
}

export const errorsInterceptorProvider: Provider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorsInterceptor,
    multi: true,
};
