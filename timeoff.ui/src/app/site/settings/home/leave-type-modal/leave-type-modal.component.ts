import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { LeaveTypeFormGroup } from '@services/company/company.service';

import { ColourPickerComponent } from '../colour-picker/colour-picker.component';

@Component({
    templateUrl: 'leave-type-modal.component.html',
    selector: 'leave-type-modal',
    imports: [ColourPickerComponent, ReactiveFormsModule]
})
export class LeaveTypeModalComponent {
    public readonly form = input.required<LeaveTypeFormGroup>();

    public readonly addedLeaveType = output();

    public addNew() {
        this.addedLeaveType.emit();
    }
}
