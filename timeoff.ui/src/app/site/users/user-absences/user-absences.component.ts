import { Component, DestroyRef, OnInit } from '@angular/core';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { UsersService } from '../../../services/users/users.service';
import { ActivatedRoute } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs';
import { CalendarService } from '../../../services/calendar/calendar.service';
import { AllowanceSummaryModel } from '../../../services/calendar/allowance-summary.model';
import { CommonModule } from '@angular/common';
import { YesPipe } from "../../../components/yes.pipe";

@Component({
    selector: 'user-absences',
    standalone: true,
    templateUrl: './user-absences.component.html',
    styleUrl: './user-absences.component.sass',
    providers: [UsersService, CalendarService],
    imports: [UserDetailsComponent, UserBreadcrumbComponent, CommonModule, YesPipe]
})
export class UserAbsencesComponent implements OnInit {
    public id: number = 0;

    public name!: string;

    public isActive!: boolean;

    public summary: AllowanceSummaryModel;

    public get usedPercent() {
        return this.summary.used / this.summary.total * 100;
    }

    public get remainingPercent() {
        return this.summary.remaining / this.summary.total * 100;
    }
    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly calendarSvc: CalendarService
    ) {
        this.summary = {
            used: 0,
            total: 0
        } as AllowanceSummaryModel
    }

    public ngOnInit(): void {
        this.route.paramMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
                    this.id = Number.parseInt(p.get('id')!);

                    return this.calendarSvc.get(new Date().getFullYear(), this.id);
                })
            )
            .subscribe((calendar) => {
                this.summary = calendar.summary;
                this.name = `${calendar.firstName} ${calendar.lastName}`;
                this.isActive = calendar.isActive;
            });
    }
}
