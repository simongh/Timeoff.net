import { Component, DestroyRef, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, RouterLink, RouterModule } from '@angular/router';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { combineLatest, switchMap } from 'rxjs';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { UsersService } from '../../../services/users/users.service';
import { CalendarComponent } from '../../../components/calendar/calendar.component';
import { CalendarService } from '../../../services/calendar/calendar.service';
import { CalendarModel } from '../../../services/calendar/calendar.model';
import { AllowanceBreakdownComponent } from '../../../components/allowance-breakdown/allowance-breakdown.component';
import { LeaveSummaryComponent } from '../../../components/leave-summary/leave-summary.component';

@Component({
    selector: 'user-calendar',
    standalone: true,
    templateUrl: './user-calendar.component.html',
    styleUrl: './user-calendar.component.sass',
    providers: [UsersService, CalendarService],
    imports: [
        RouterLink,
        UserBreadcrumbComponent,
        UserDetailsComponent,
        CalendarComponent,
        AllowanceBreakdownComponent,
        LeaveSummaryComponent,
    ],
})
export class UserCalendarComponent implements OnInit {
    public id: number = 0;

    public currentYear!: number;

    public get nextYear() {
        return this.currentYear + 1;
    }

    public get lastYear() {
        return this.currentYear - 1;
    }

    public start!: Date;

    public calendar: CalendarModel;

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly calendarSvc: CalendarService
    ) {
        this.calendar = {
            holidays: [],
            firstName: '',
            lastName: '',
            isActive: true,
            summary: {
                available: 0,
                carryOver: 0,
                used: 0,
                previousYear: 0,
                allowance: 0,
                isAccrued: false,
                adjustment: 0,
                employmentRangeAdjustment: 0,
                accruedAdjustment: 0,
                total: 0,
                remaining: 0,
                leaveSummary: [],
            },
        } as CalendarModel;
    }

    public ngOnInit(): void {
        combineLatest([this.route.paramMap, this.route.queryParamMap])
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(([p, q]) => {
                    this.id = Number.parseInt(p.get('id')!);

                    if (q.has('year')) {
                        this.currentYear = Number.parseInt(q.get('year')!);
                    } else {
                        this.currentYear = new Date().getFullYear();
                    }

                    this.start = new Date(this.currentYear, 0, 1);

                    return this.calendarSvc.get(this.currentYear, this.id);
                })
            )
            .subscribe((calendar) => {
                this.calendar = calendar;
            });
    }
}
