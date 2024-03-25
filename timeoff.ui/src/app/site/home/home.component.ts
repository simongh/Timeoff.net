import { Component, DestroyRef, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { switchMap } from 'rxjs';
import { startOfMonth, startOfYear } from 'date-fns';

import { FlashComponent } from '@components/flash/flash.component';
import { CalendarComponent } from '@components/calendar/calendar.component';
import { AllowanceBreakdownComponent } from '@components/allowance-breakdown/allowance-breakdown.component';
import { LeaveSummaryComponent } from '@components/leave-summary/leave-summary.component';

import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';
import { CalendarService } from '@services/calendar/calendar.service';

import { PublicHolidayModel } from '@models/public-holiday.model';

@Component({
    standalone: true,
    templateUrl: 'home.component.html',
    imports: [
        FlashComponent,
        CommonModule,
        RouterLink,
        CalendarComponent,
        AllowanceBreakdownComponent,
        LeaveSummaryComponent,
    ],
    providers: [CalendarService],
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

    public allowanceSummary = {} as AllowanceSummaryModel;

    public holidays: PublicHolidayModel[] = [];

    public managerName: string = 'manager';

    public managerEmail: string = 'manager@email';

    public teamName: string = 'team';

    public teamId: number = 0;

    public start!: Date;

    constructor(
        private readonly route: ActivatedRoute,
        private readonly destroyed: DestroyRef,
        private readonly calendarSvc: CalendarService
    ) {}

    public ngOnInit(): void {
        this.route.queryParamMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
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

                    return this.calendarSvc.get(this.year);
                })
            )
            .subscribe((calendar) => {
                this.allowanceSummary = calendar.summary;
                this.holidays = calendar.holidays;
                this.name = `${calendar.firstName} ${calendar.lastName}`;
            });
    }
}
