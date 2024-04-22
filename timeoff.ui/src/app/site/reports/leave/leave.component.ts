import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { FlashComponent } from '@components/flash/flash.component';
import { TeamSelectComponent } from '@components/team-select/team-select.component';
import { DatePickerDirective } from '@components/date-picker.directive';

import { CompanyService } from '@services/company/company.service';
import { LeaveTypeModel } from '@services/company/leave-type.model';

import { LeaveResultModel } from './leave-result.model';
import { LeaveService } from './leave.service';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
    selector: 'leave',
    standalone: true,
    templateUrl: './leave.component.html',
    styleUrl: './leave.component.scss',
    imports: [RouterLink, FlashComponent, TeamSelectComponent, CommonModule, DatePickerDirective, ReactiveFormsModule],
    providers: [CompanyService, LeaveService],
})
export class LeaveComponent implements OnInit {
    protected readonly results = signal<LeaveResultModel[]>([]);

    protected readonly leaveTypes = signal<LeaveTypeModel[]>([]);

    protected get form() {
        return this.leaveSvc.form;
    }

    protected readonly submitting = signal(false);
    
    constructor(
        private readonly companySvc: CompanyService,
        private readonly leaveSvc: LeaveService,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        this.companySvc
            .getLeaveTypes()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((data) => {
                this.leaveTypes.set(data);
            });
    }
}
