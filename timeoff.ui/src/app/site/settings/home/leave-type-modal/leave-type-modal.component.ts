import { Component, EventEmitter, Output } from "@angular/core";
import { ColourPickerComponent } from "../colour-picker/colour-picker.component";
import { FormGroup } from "@angular/forms";
import { LeaveTypeControls } from "../settings-service";

@Component({
    standalone: true,
    templateUrl: 'leave-type-modal.component.html',
    selector: 'leave-type-modal',
    imports: [ColourPickerComponent]
})
export class LeaveTypeModalComponent {
    @Output()
    public addedLeaveType = new EventEmitter<FormGroup<LeaveTypeControls>>();

    public addNew()
    {
        return true;
    }
}
