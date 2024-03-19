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
import { YesPipe } from '../../../components/yes.pipe';
import { LeaveRequestModel } from '../../../models/leave-request.model';
import { RequestsListComponent } from '../../../components/requests-list/requests-list.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MessagesService } from '../../../services/messages/messages.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'user-absences',
    standalone: true,
    templateUrl: './user-absences.component.html',
    styleUrl: './user-absences.component.sass',
    providers: [UsersService, CalendarService],
    imports: [
        UserDetailsComponent,
        UserBreadcrumbComponent,
        CommonModule,
        YesPipe,
        RequestsListComponent,
        ReactiveFormsModule,
    ],
})
export class UserAbsencesComponent implements OnInit {
    public id: number = 0;

    public get form() {
        return this.usersSvc.adjustmentForm;
    }

    public name!: string;

    public isActive!: boolean;

    public summary: AllowanceSummaryModel;

    public get usedPercent() {
        return (this.summary.used / this.summary.total) * 100;
    }

    public get remainingPercent() {
        return (this.summary.remaining / this.summary.total) * 100;
    }

    public get groupedRequests() {
        return Object.entries(
            this.leave.reduce((groups, item) => {
                const year = item.startDate.getFullYear();

                (groups[year] ||= []).push(item);

                return groups;
            }, {} as Record<number, LeaveRequestModel[]>)
        );
    }

    public submitting = false;

    private leave: LeaveRequestModel[] = [];

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly calendarSvc: CalendarService,
        private readonly msgsSvc: MessagesService
    ) {
        this.summary = {
            used: 0,
            total: 0,
        } as AllowanceSummaryModel;
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

                this.usersSvc.fillAdjustments(calendar.summary);
            });
    }

    public save() {
        this.submitting = true;

        this.usersSvc
            .updateAdjustments(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Adjustments save');
                    this.submitting = false;
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.isError('Unable to save adjustments');
                    this.submitting = false;
                },
            });
    }
}
