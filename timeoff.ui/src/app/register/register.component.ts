import { Component, DestroyRef, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest } from 'rxjs';

import { FlashComponent } from '@components/flash/flash.component';
import { compareValidator, listValidator } from '@components/validators';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { MessagesService } from '@services/messages/messages.service';
import { TimeZoneModel } from '@services/company/time-zone.model';
import { Country } from '@services/company/country.model';
import { CompanyService } from '@services/company/company.service';

import { RegisterModel } from './register.model';
import { RegisterService } from './register.service';

@Component({
    selector: 'register-page',
    standalone: true,
    templateUrl: 'register.component.html',
    providers: [RegisterService],
    imports: [FlashComponent, ReactiveFormsModule, CommonModule, ValidatorMessageComponent],
})
export class RegisterComponent implements OnInit {
    public registerForm = this.fb.group(
        {
            companyName: ['', [Validators.required]],
            firstName: ['', [Validators.required]],
            lastName: ['', [Validators.required]],
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(8)]],
            confirmPassword: ['', [Validators.required]],
            country: ['GB'],
            timezone: [''],
        },
        {
            validators: [compareValidator('password', 'confirmPassword')],
        }
    );

    public countries!: Country[];

    public timezones!: TimeZoneModel[];

    public submitting = false;

    constructor(
        private readonly fb: FormBuilder,
        private readonly registerSvc: RegisterService,
        private readonly companySvc: CompanyService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {
        ({ countries: this.countries, timezones: this.timezones } = this.registerSvc.getOptions());
    }

    public ngOnInit(): void {
        combineLatest([this.companySvc.countries(), this.companySvc.timeZones()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([countries, timezones]) => {
                this.countries = countries;
                this.timezones = timezones;

                this.registerForm.controls.country.addValidators(listValidator(this.countries.map((c) => c.code)));
                this.registerForm.controls.timezone.addValidators(listValidator(this.timezones.map((t) => t.name)));

                this.registerForm.controls.timezone.setValue(this.timezones[0].name);
            });
    }

    public register() {
        this.registerForm.markAllAsTouched();
        if (this.registerForm.invalid) {
            return;
        }

        this.submitting = true;
        this.registerSvc
            .register(this.registerForm.value as RegisterModel)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess(
                        'Company registered successfully. Please login using the details you supplied'
                    );
                    this.registerForm.reset();
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.hasErrors(e.error.errors);
                },
            });
    }
}
