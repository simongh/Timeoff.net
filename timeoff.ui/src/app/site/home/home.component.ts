import { Component, DestroyRef, OnInit } from '@angular/core';
import { FlashComponent } from '../../components/flash/flash.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CalendarComponent } from '../../components/calendar/calendar.component';
import { startOfMonth, startOfYear } from 'date-fns';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AllowanceSummaryModel } from '../../services/calendar/allowance-summary.model';
import { AllowanceBreakdownComponent } from "../../components/allowance-breakdown/allowance-breakdown.component";
import { LeaveSummaryComponent } from "../../components/leave-summary/leave-summary.component";

@Component({
    standalone: true,
    templateUrl: 'home.component.html',
    imports: [FlashComponent, CommonModule, RouterLink, CalendarComponent, AllowanceBreakdownComponent, LeaveSummaryComponent]
})
export class HomeComponent implements OnInit {
    public name: string = '';

    public get year(): number {
        return this.start.getFullYear();
    }

    public get nextYear(): number {
        return this.start.getFullYear() + 1;
    }

    public get lastYear(): number {
        return this.start.getFullYear() - 1;
    }

    public showFullYear: boolean = false;

    public allowanceSummary = {
        total: 0,
        remaining: 0,
        allowance: 0,
        carryOver: 0,
        adjustment: 0,
        employmentRangeAdjustment: 0,
        used: 0,
    } as AllowanceSummaryModel;

    public managerName: string = 'manager';

    public managerEmail: string = 'manager@email';

    public teamName: string = 'team';

    public teamId: number = 0;

    public start!: Date;

    constructor(private readonly route: ActivatedRoute, private readonly destroyed: DestroyRef) {}

    public ngOnInit(): void {
        this.route.queryParamMap.pipe(takeUntilDestroyed(this.destroyed)).subscribe((p) => {
            this.showFullYear = p.has('showFullYear');

            if (this.showFullYear) {
                if (p.has('year')) {
                    this.start = startOfYear(new Date(Number.parseInt(p.get('year')!), 0, 1));
                } else {
                    this.start = startOfYear(new Date());
                }
            } else {
                this.start = startOfMonth(new Date());
            }
        });
    }
}
