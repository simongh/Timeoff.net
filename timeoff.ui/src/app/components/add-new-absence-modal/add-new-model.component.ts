import { Component, DestroyRef, signal } from '@angular/core';
import { computedAsync } from 'ngxtension/computed-async';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { datePartList } from '@models/types';

import { DatePickerDirective } from '@components/date-picker.directive';

import { CompanyService } from '@services/company/company.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { MessagesService } from '@services/messages/messages.service';

import { BookingService } from './booking.service';

@Component({
    standalone: true,
    selector: 'add-new-absence-modal',
    templateUrl: 'add-new-modal.component.html',
    providers: [BookingService, CompanyService],
    imports: [ReactiveFormsModule, CommonModule, DatePickerDirective],
})
export class AddNewModalComponent {
    protected readonly parts = signal(datePartList()).asReadonly();

    protected readonly leaveTypes = computedAsync(() => this.companySvc.getLeaveTypes(), { initialValue: [] });

    protected get form() {
        return this.bookingSvc.form;
    }

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected readonly users = computedAsync(() => this.companySvc.getUsers(), { initialValue: [] });

    protected readonly submitting = signal(false);

    constructor(
        private readonly companySvc: CompanyService,
        private readonly bookingSvc: BookingService,
        private readonly currentUser: LoggedInUserService,
        private readonly msgSvc: MessagesService,
        private destroyed: DestroyRef
    ) {}

    public cancel() {
        this.bookingSvc.reset();
    }

    public add() {
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }

        this.submitting.set(true);

        this.bookingSvc
            .add()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.bookingSvc.reset();
                    this.msgSvc.isSuccess('New absence request was added');
                    this.submitting.set(false);
                },
            });
    }
}
