import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';

import { compareValidator } from '@components/validators';

@Injectable({ providedIn: 'root' })
export class RegisterService {
    readonly #fb = inject(FormBuilder);

    readonly #client = inject(HttpClient);

    public readonly form = this.#fb.group(
        {
            companyName: ['', [Validators.required]],
            firstName: ['', [Validators.required]],
            lastName: ['', [Validators.required]],
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(8)]],
            confirmPassword: ['', [Validators.required]],
            country: ['GB'],
            timezone: ['', [Validators.required]],
        },
        {
            validators: [compareValidator('password', 'confirmPassword')],
        }
    );

    public register(): Observable<void> {
        return this.#client.post<void>('/api/account/register', this.form.value);
    }
}
