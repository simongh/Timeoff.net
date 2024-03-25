import { Component, EventEmitter, Output } from '@angular/core';
import { ColourPickerComponent } from '../colour-picker/colour-picker.component';

import { LeaveTypeFormGroup } from '@services/company/company.service';

@Component({
    standalone: true,
    templateUrl: 'leave-type-modal.component.html',
    selector: 'leave-type-modal',
    imports: [ColourPickerComponent],
})
export class LeaveTypeModalComponent {
    @Output()
    public addedLeaveType = new EventEmitter<LeaveTypeFormGroup>();

    public addNew() {
        return true;
    }
}
