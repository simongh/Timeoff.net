import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, computed, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { formatDate } from 'date-fns';
import { computedAsync } from 'ngxtension/computed-async';

import { FlashComponent } from '@components/flash/flash.component';
import { DatePickerDirective } from '@components/date-picker.directive';
import { TeamSelectComponent } from '@components/team-select/team-select.component';

import { CompanyService } from '@services/company/company.service';
import { LeaveTypeModel } from '@services/company/leave-type.model';

import { AllowanceModel } from './allowance.model';
import { AllowanceUsageService } from './allowance-usage.service';
import { map, switchMap } from 'rxjs';

@Component({
    selector: 'allowance-usage',
    standalone: true,
    templateUrl: './allowance-usage.component.html',
    styleUrl: './allowance-usage.component.scss',
    providers: [CompanyService, AllowanceUsageService],
    imports: [FlashComponent, CommonModule, RouterLink, ReactiveFormsModule, DatePickerDirective, TeamSelectComponent],
})
export class AllowanceUsageComponent implements OnInit {
    protected readonly results = signal<AllowanceModel[]>([]);

    public get form() {
        return this.allowanceSvc.form;
    }

    protected readonly prettyDateRange = computed(() => {
        const parts = Array<string>();
        parts.push(formatDate(this.start(), 'LLL'));

        if (this.start().getMonth() != this.end().getMonth()) {
            parts.push('-');
            parts.push(formatDate(this.end(), 'MMM'));
        }
        parts.push(formatDate(this.end(), 'yyyy'));

        return parts.join(' ');
    });

    protected readonly leaveTypes = signal<LeaveTypeModel[]>([]);

    protected readonly submitting = signal(false);

    private readonly start = signal(new Date());

    private readonly end = signal(new Date());

    constructor(
        private destroyed: DestroyRef,
        private readonly allowanceSvc: AllowanceUsageService,
        private readonly companySvc: CompanyService
    ) {}

    public ngOnInit(): void {
        this.companySvc
            .getLeaveTypes()
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((leaveTypes) => {
                    return this.allowanceSvc.getResults().pipe(
                        map((results) => ({
                            results: results,
                            leaveTypes: leaveTypes,
                        }))
                    );
                })
            )
            .subscribe((data) => {
                this.leaveTypes.set(data.leaveTypes);
                this.loadResults(data.results);
            });
    }

    public update() {
        if (this.form.invalid) {
            return;
        }

        this.submitting.set(true);

        this.allowanceSvc
            .getResults()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((data) => {
                this.loadResults(data);
                this.submitting.set(false);
            });
    }

    private loadResults(data: AllowanceModel[]) {
        this.results.set(data.map((d) => {
            d.totals = this.leaveTypes().map((l) => d.leaveSummary.find((s) => s.id == l.id)?.allowanceUsed ?? 0);

            return d;
        }));

        this.start.set(this.allowanceSvc.start);
        this.end.set(this.allowanceSvc.end);
    }
}
