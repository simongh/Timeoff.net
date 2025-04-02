import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, computed, effect, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { formatDate } from 'date-fns';
import { map, switchMap } from 'rxjs';
import { TippyDirective } from '@ngneat/helipopper';

import { FlashComponent } from '@components/flash/flash.component';

import { CompanyService } from '@services/company/company.service';
import { LeaveTypeModel } from '@services/company/leave-type.model';

import { AllowanceModel } from './allowance.model';
import { AllowanceUsageService } from './allowance-usage.service';
import { FilterComponent } from './filter.component';
import { derivedAsync } from 'ngxtension/derived-async';

@Component({
    selector: 'allowance-usage',
    templateUrl: './allowance-usage.component.html',
    styleUrl: './allowance-usage.component.scss',
    providers: [CompanyService, AllowanceUsageService],
    imports: [FlashComponent, CommonModule, RouterLink, ReactiveFormsModule, TippyDirective, FilterComponent],
})
export class AllowanceUsageComponent {
    readonly #destroyed = inject(DestroyRef);

    readonly #allowanceSvc = inject(AllowanceUsageService);

    readonly #companySvc = inject(CompanyService);

    protected readonly results = signal<AllowanceModel[]>([]);

    protected get form() {
        return this.#allowanceSvc.form;
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

    protected readonly leaveTypes = derivedAsync(
        () => this.#companySvc.getLeaveTypes().pipe(takeUntilDestroyed(this.#destroyed)),
        { initialValue: [] }
    );

    protected readonly submitting = signal(false);

    private readonly start = computed(() => this.#allowanceSvc.start);

    private readonly end = computed(() => this.#allowanceSvc.end);

    constructor() {
        effect(() => {
            this.#allowanceSvc
                .getResults()
                .pipe(takeUntilDestroyed(this.#destroyed))
                .subscribe((data) => {
                    this.loadResults(data);
                });
        });
    }

    public update() {
        if (this.form.invalid) {
            return;
        }

        this.submitting.set(true);

        this.#allowanceSvc
            .getResults()
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe((data) => {
                this.loadResults(data);
                this.submitting.set(false);
            });
    }

    private loadResults(data: AllowanceModel[]) {
        this.results.set(
            data.map((d) => {
                d.totals = this.leaveTypes().map((l) => d.leaveSummary.find((s) => s.id == l.id)?.allowanceUsed ?? 0);

                return d;
            })
        );
    }
}
