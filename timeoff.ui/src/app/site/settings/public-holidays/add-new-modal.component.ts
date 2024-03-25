import { Component, DestroyRef, EventEmitter, Input, Output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';

import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { DatePickerDirective } from '@components/date-picker.directive';

import { MessagesService } from '@services/messages/messages.service';

import { PublicHolidaysService } from './public-holidays.service';

@Component({
    standalone: true,
    templateUrl: 'add-new-modal.component.html',
    selector: 'add-new-modal',
    imports: [ReactiveFormsModule, ValidatorMessageComponent, DatePickerDirective],
})
export class AddNewModalComponent {
    public get form() {
        return this.holidaySvc.addForm!;
    }

    public dateFormat = 'yyyy-mm-dd';

    @Input()
    public set year(value: number) {
        this.holidaySvc.setAddForm(value);
    }

    @Output()
    public added = new EventEmitter();

    public submitting = false;

    constructor(
        private readonly holidaySvc: PublicHolidaysService,
        private readonly msgsSvc: MessagesService,
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

        this.submitting = true;
        this.holidaySvc
            .addNew()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Holiday added');

                    this.added.emit();
                    this.form.reset();
                    this.submitting = false;
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to add holiday');
                    }

                    this.added.emit();
                    this.submitting = false;
                },
            });
    }
}
