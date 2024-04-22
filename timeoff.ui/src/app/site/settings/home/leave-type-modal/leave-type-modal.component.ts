import { Component, EventEmitter, Input, Output, input } from '@angular/core';
import { ColourPickerComponent } from '../colour-picker/colour-picker.component';

import { LeaveTypeFormGroup } from '@services/company/company.service';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
    standalone: true,
    templateUrl: 'leave-type-modal.component.html',
    selector: 'leave-type-modal',
    imports: [ColourPickerComponent, ReactiveFormsModule],
})
export class LeaveTypeModalComponent {
    public readonly form = input.required<LeaveTypeFormGroup>();

    @Output()
    public addedLeaveType = new EventEmitter();

    public addNew() {
        this.addedLeaveType.emit();
    }
}
