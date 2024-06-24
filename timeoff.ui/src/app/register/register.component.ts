import { Component, DestroyRef, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { derivedAsync } from 'ngxtension/derived-async';

import { FlashComponent } from '@components/flash/flash.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

import { MessagesService } from '@services/messages/messages.service';
import { CompanyService } from '@services/company/company.service';

import { RegisterService } from './register.service';

@Component({
    selector: 'register-page',
    standalone: true,
    templateUrl: 'register.component.html',
    providers: [RegisterService, CompanyService],
    imports: [FlashComponent, ReactiveFormsModule, CommonModule, ValidatorMessageComponent],
})
export class RegisterComponent {
    protected get registerForm() {
        return this.registerSvc.form;
    }

    protected readonly countries = derivedAsync(() => this.companySvc.countries(), { initialValue: [] });

    protected readonly timezones = derivedAsync(() => this.companySvc.timeZones(), { initialValue: [] });

    protected readonly submitting = signal(false);

    constructor(
        private readonly registerSvc: RegisterService,
        private readonly companySvc: CompanyService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {}

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
            });
    }
}
