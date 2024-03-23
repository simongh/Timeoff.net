import { DestroyRef, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';
import { RegisterOptions } from './register-options.model';
import { RegisterModel } from './register.model';

@Injectable()
export class RegisterService {
    constructor(private readonly client: HttpClient, private destroyed: DestroyRef) {}

    public getOptions(): RegisterOptions {
        return {
            countries: [],
            timezones: [],
        };
    }

    public register(model: RegisterModel): Observable<void> {
        return this.client
            .post<void>('/api/register', {
                ...model,
                confirmPassword: model.password,
            })
            .pipe(takeUntilDestroyed(this.destroyed));
    }
}
