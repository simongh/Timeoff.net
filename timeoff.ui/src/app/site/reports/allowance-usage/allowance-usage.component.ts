import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { formatDate, parseISO } from 'date-fns';

import { FlashComponent } from '@components/flash/flash.component';
import { DatePickerDirective } from '@components/date-picker.directive';
import { TeamSelectComponent } from "@components/team-select/team-select.component";

import { CompanyService } from '@services/company/company.service';
import { LeaveTypeModel } from '@services/company/leave-type.model';

import { AllowanceModel } from './allowance.model';
import { AllowanceUsageService } from './allowance-usage.service';

@Component({
    selector: 'allowance-usage',
    standalone: true,
    templateUrl: './allowance-usage.component.html',
    styleUrl: './allowance-usage.component.scss',
    providers: [CompanyService, AllowanceUsageService],
    imports: [FlashComponent, CommonModule, RouterLink, ReactiveFormsModule, DatePickerDirective, TeamSelectComponent]
})
export class AllowanceUsageComponent implements OnInit {
    public results: AllowanceModel[] = [];

    public get form() {
        return this.allowanceSvc.form;
    }

    public get prettyDateRange() {
        const parts = Array<string>();
        parts.push(formatDate(this.start,'LLL'))

        if (this.form.value.start != this.form.value.end) {
            parts.push('-');
            parts.push(formatDate(this.end, 'MMM'));
        }
        parts.push(formatDate(this.end,'yyyy'));

        return parts.join(' ')
    }

    public leaveTypes: LeaveTypeModel[] = [];

    private get start() {
        return parseISO(this.form.value.start!);
    }
    
    private get end() {
        return parseISO(this.form.value.end!);
    }

    constructor(
        private destroyed: DestroyRef,
        private readonly allowanceSvc: AllowanceUsageService,
        private readonly companySvc: CompanyService,
    ) {}

    public ngOnInit(): void {
        this.companySvc.getLeaveTypes()
        .pipe(takeUntilDestroyed(this.destroyed))
        .subscribe((leaveTypes)=>{

        })
    }
}
