import { CommonModule } from '@angular/common';
import { Component, DestroyRef, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs';

import { CompanyService } from '@services/company/company.service';

import { ColourPickerComponent } from '../colour-picker/colour-picker.component';
import { LeaveTypeModalComponent } from '../leave-type-modal/leave-type-modal.component';

@Component({
    selector: 'leave-types',
    templateUrl: './leave-types.component.html',
    styleUrl: './leave-types.component.scss',
    providers: [],
    imports: [ColourPickerComponent, CommonModule, ReactiveFormsModule, LeaveTypeModalComponent]
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

        this.companySvc
            .updateLeaveTypes()
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(() => {
                    return this.companySvc.getLeaveTypes();
                })
            )
            .subscribe((lt) => {
                this.companySvc.leaveTypes.clear();
                this.companySvc.fillLeaveTypes(lt);
            });
    }

    public removeLeaveType(id: number) {
        this.companySvc
            .removeLeaveType(id)
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(() => {
                    return this.companySvc.getLeaveTypes();
                })
            )
            .subscribe((lt) => {
                this.companySvc.leaveTypes.clear();
                this.companySvc.fillLeaveTypes(lt);
            });
    }
}
