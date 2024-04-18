import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest } from 'rxjs';

import { FlashComponent } from '@components/flash/flash.component';
import { listValidator } from '@components/validators';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { MessagesService } from '@services/messages/messages.service';
import { TimeZoneModel } from '@services/company/time-zone.model';
import { Country } from '@services/company/country.model';
import { CompanyService } from '@services/company/company.service';

import { RegisterService } from './register.service';

@Component({
    selector: 'register-page',
    standalone: true,
    templateUrl: 'register.component.html',
    providers: [RegisterService],
    imports: [FlashComponent, ReactiveFormsModule, CommonModule, ValidatorMessageComponent],
})
export class RegisterComponent implements OnInit {
    protected get registerForm() {
        return this.registerSvc.form;
    }

    protected readonly countries = signal<Country[]>([]);

    protected readonly timezones = signal<TimeZoneModel[]>([]);

    protected submitting = signal(false);

    constructor(
        private readonly registerSvc: RegisterService,
        private readonly companySvc: CompanyService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        combineLatest([this.companySvc.countries(), this.companySvc.timeZones()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([countries, timezones]) => {
                this.countries.set(countries);
                this.timezones.set(timezones);

                this.registerForm.controls.country.addValidators(listValidator(this.countries().map((c) => c.code)));
                this.registerForm.controls.timezone.addValidators(listValidator(this.timezones().map((t) => t.name)));
            });
    }

    public register() {
        this.registerForm.markAllAsTouched();
        if (this.registerForm.invalid) {
            return;
        }

        this.submitting.set(true);
        this.registerSvc
            .register()
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

                    this.submitting.set(false);
                },
            });
    }
}
