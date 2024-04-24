import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { computedAsync } from 'ngxtension/computed-async';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest } from 'rxjs';

import { datePartList } from '@models/types';

import { DatePickerDirective } from '@components/date-picker.directive';

import { CompanyService } from '@services/company/company.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { MessagesService } from '@services/messages/messages.service';
import { LeaveTypeModel } from '@services/company/leave-type.model';

import { BookingService } from './booking.service';
import { UserModel } from '@services/company/user.model';
import { ValidatorMessageComponent } from "../validator-message/validator-message.component";

@Component({
    standalone: true,
    selector: 'add-new-absence-modal',
    templateUrl: 'add-new-modal.component.html',
    providers: [BookingService, CompanyService],
    imports: [ReactiveFormsModule, CommonModule, DatePickerDirective, ValidatorMessageComponent]
})
export class AddNewModalComponent implements OnInit {
    protected readonly parts = signal(datePartList()).asReadonly();

    protected readonly leaveTypes = signal<LeaveTypeModel[]>([]);

    protected get form() {
        return this.bookingSvc.form;
    }

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected readonly users = signal<UserModel[]>([]);

    protected readonly submitting = signal(false);

    constructor(
        private readonly companySvc: CompanyService,
        private readonly bookingSvc: BookingService,
        private readonly currentUser: LoggedInUserService,
        private readonly msgSvc: MessagesService,
        private destroyed: DestroyRef
    ) {
        currentUser.refresh$.pipe(takeUntilDestroyed()).subscribe(() => this.fillForm());
    }

    public ngOnInit(): void {
        this.fillForm();
    }

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

    private fillForm() {
        combineLatest([this.companySvc.getLeaveTypes(), this.companySvc.getUsers()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([leaveTypes, users]) => {
                this.leaveTypes.set(leaveTypes);
                this.users.set(users);
            });
    }
}
