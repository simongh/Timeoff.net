import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { formatDate, parse, parseISO } from 'date-fns';

import { FlashComponent } from '@components/flash/flash.component';
import { DatePickerDirective } from '@components/date-picker.directive';

import { CompanyService } from '@services/company/company.service';
import { TeamModel } from '@services/company/team.model';

import { AllowanceModel } from './allowance.model';
import { AllowanceUsageService } from './allowance-usage.service';

@Component({
    selector: 'allowance-usage',
    standalone: true,
    imports: [FlashComponent, CommonModule, RouterLink, ReactiveFormsModule, DatePickerDirective],
    templateUrl: './allowance-usage.component.html',
    styleUrl: './allowance-usage.component.scss',
    providers: [CompanyService, AllowanceUsageService],
})
export class AllowanceUsageComponent implements OnInit {
    public teams: TeamModel[] = [];

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

    private get start() {
        return parseISO(this.form.value.start!);
    }
    
    private get end() {
        return parseISO(this.form.value.end!);
    }

    constructor(
        private destroyed: DestroyRef,
        private readonly companySvc: CompanyService,
        private readonly allowanceSvc: AllowanceUsageService
    ) {}

    public ngOnInit(): void {
        this.companySvc
            .getTeams()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((data) => {
                this.teams = data;
            });
    }
}
