import { Component, EventEmitter, Input, Output } from "@angular/core";
import { PublicHolidaysService } from "../../../services/public-holidays/public-holidays.service";
import { ReactiveFormsModule } from "@angular/forms";
import { ValidatorMessageComponent } from "../../../components/validator-message/validator-message.component";
import { DatePickerDirective } from "./date-picker.directive";
import { PublicHolidayModel } from "../../../services/public-holidays/public-holiday.model";

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
    public added = new EventEmitter<PublicHolidayModel>();

    constructor(private readonly holidaySvc: PublicHolidaysService) {}

    public cancel() {
        this.form.reset();
    }

    public create() {
        this.form.markAllAsTouched();

        console.log(this.form.value);
        if (this.form.invalid) {
            return;
        }

        this.added.emit(this.form.value as PublicHolidayModel);

        this.form.reset();

        return true;
    }
}