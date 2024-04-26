import { CommonModule } from '@angular/common';
import { Component, DestroyRef, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { CompanyService } from '@services/company/company.service';

import { ColourPickerComponent } from '../colour-picker/colour-picker.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LeaveTypeModalComponent } from '../leave-type-modal/leave-type-modal.component';

@Component({
    selector: 'leave-types',
    standalone: true,
    templateUrl: './leave-types.component.html',
    styleUrl: './leave-types.component.scss',
    providers: [],
    imports: [ColourPickerComponent, CommonModule, ReactiveFormsModule, LeaveTypeModalComponent],
})
export class LeaveTypesComponent {
    protected get leaveTypes() {
        return this.companySvc.leaveTypes;
    }

    protected get leaveTypeForm() {
        return this.companySvc.leaveTypeForm;
    }

    protected get firstType() {
        return this.companySvc.first;
    }

    protected readonly submitting = signal(false);

    constructor(private readonly companySvc: CompanyService, private destroyed: DestroyRef) {}

    public updateLeaveTypes() {
        this.companySvc.updateLeaveTypes().pipe(takeUntilDestroyed(this.destroyed)).subscribe();
    }

    public addLeaveType() {
        this.leaveTypes.push(this.leaveTypeForm);

        this.companySvc.resetLeaveTypeForm();
    }

    public removeLeaveType(id: number) {}
}
