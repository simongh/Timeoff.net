import { DestroyRef, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';

import { compareValidator } from '@components/validators';

type RegisterFormGroup = ReturnType<RegisterService['createForm']>;

@Injectable()
export class RegisterService {
    public readonly form: RegisterFormGroup;

    constructor(private readonly fb: FormBuilder, private readonly client: HttpClient) {
        this.form = this.createForm();
    }

    public register(): Observable<void> {
        return this.client.post<void>('/api/register', this.form.value);
    }

    private createForm() {
        const form = this.fb.group(
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

        return form;
    }
}
