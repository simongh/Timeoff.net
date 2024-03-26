import { Component, EventEmitter, Input, Output } from '@angular/core';
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
    @Input()
    public form!: LeaveTypeFormGroup;

    @Output()
    public addedLeaveType = new EventEmitter();

    public addNew() {
        this.addedLeaveType.emit();
    }
}
