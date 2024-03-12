import { Component, DestroyRef, EventEmitter, Input, Output } from "@angular/core";
import { PublicHolidaysService } from "../../../services/public-holidays/public-holidays.service";
import { ReactiveFormsModule } from "@angular/forms";
import { ValidatorMessageComponent } from "../../../components/validator-message/validator-message.component";
import { DatePickerDirective } from "./date-picker.directive";
import { PublicHolidayModel } from "../../../services/public-holidays/public-holiday.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FlashModel, hasErrors, isError, isSuccess } from "../../../components/flash/flash.model";
import { HttpErrorResponse } from "@angular/common/http";

@Component({
    standalone: true,
    templateUrl: 'add-new-modal.component.html',
    selector: 'add-new-modal',
    imports: [ReactiveFormsModule,ValidatorMessageComponent, DatePickerDirective]
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
    public added = new EventEmitter<FlashModel>();

    public submitting = false;

    constructor(
        private readonly holidaySvc: PublicHolidaysService,
        private destroyed: DestroyRef,
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
        this.holidaySvc.addNew()
            .pipe(
                takeUntilDestroyed(this.destroyed),
            ).subscribe({
                next: () => {
                    this.added.emit(isSuccess('Holiday added'));
                    this.form.reset();
                    this.submitting = false;
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.added.emit(hasErrors(e.error.errors));
                    } else {
                        this.added.emit(isError('Unable to add holiday'));
                    }
                    this.submitting = false;
                } 
            });
    }
}