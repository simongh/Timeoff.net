import { Component, DestroyRef, EventEmitter, Output, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { DatePickerDirective } from '@components/date-picker.directive';

import { MessagesService } from '@services/messages/messages.service';

import { PublicHolidaysService } from './public-holidays.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    standalone: true,
    templateUrl: 'add-new-modal.component.html',
    selector: 'add-new-modal',
    imports: [ReactiveFormsModule, ValidatorMessageComponent, DatePickerDirective],
})
export class AddNewModalComponent {
    protected get form() {
        return this.holidaySvc.addForm!;
    }

    protected readonly dateFormat = this.currentUser.dateFormat;

    @Output()
    public readonly added = new EventEmitter();

    protected readonly submitting = signal(false);

    constructor(
        private readonly holidaySvc: PublicHolidaysService,
        private readonly msgsSvc: MessagesService,
        private readonly currentUser: LoggedInUserService,
        private destroyed: DestroyRef
    ) {}

    public cancel() {
        this.form.reset();
    }

    public create() {
        this.form.markAllAsTouched();

        console.log(this.form.value);
        if (this.form.invalid) {
            return;
        }

        this.submitting.set(true);
        this.holidaySvc
            .addNew()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Holiday added');

                    this.added.emit();
                    this.form.reset();
                    this.submitting.set(false);
                },
            });
    }
}
