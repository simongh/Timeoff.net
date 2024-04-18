import { ChangeDetectorRef, Component, DestroyRef, computed, effect, numberAttribute, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { injectParams } from 'ngxtension/inject-params';

import { YesPipe } from '@components/yes.pipe';
import { RequestsListComponent } from '@components/requests-list/requests-list.component';

import { CalendarService } from '@services/calendar/calendar.service';
import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';
import { MessagesService } from '@services/messages/messages.service';

import { UsersService } from '../users.service';
import { LeaveRequestModel } from '../../../models/leave-request.model';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { UserDetailsComponent } from '../user-details/user-details.component';

@Component({
    selector: 'user-absences',
    standalone: true,
    templateUrl: './user-absences.component.html',
    styleUrl: './user-absences.component.scss',
    providers: [UsersService, CalendarService],
    imports: [
        UserDetailsComponent,
        UserBreadcrumbComponent,
        CommonModule,
        YesPipe,
        RequestsListComponent,
        ReactiveFormsModule,
        RouterLink,
    ],
})
export class UserAbsencesComponent {
    public id = injectParams((p) => numberAttribute(p['id']));

    public get form() {
        return this.usersSvc.adjustmentForm;
    }

    public name = signal('');

    public isActive = signal(false);

    public summary =signal({} as AllowanceSummaryModel);

    public usedPercent = computed(()=>(this.summary().used / this.summary().total) * 100);

    public remainingPercent = computed(()=> (this.summary().remaining / this.summary().total) * 100);

    public get groupedRequests() {
        return Object.entries(
            this.leave.reduce((groups, item) => {
                const year = item.startDate.getFullYear();

                (groups[year] ||= []).push(item);

                return groups;
            }, {} as Record<number, LeaveRequestModel[]>)
        );
    }

    public submitting = signal(false);

    private leave: LeaveRequestModel[] = [];

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly calendarSvc: CalendarService,
        private readonly msgsSvc: MessagesService,
        private readonly cd: ChangeDetectorRef
    ) {
        effect(()=> {
            this.calendarSvc.get(new Date().getFullYear(), this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((calendar) => {
                this.summary.set(calendar.summary);
                this.name.set(`${calendar.firstName} ${calendar.lastName}`);
                this.isActive.set(calendar.isActive);

                this.usersSvc.fillAdjustments(calendar.summary);
            });
        })
    }

    public save() {
        this.submitting.set(true);

        this.usersSvc
            .updateAdjustments(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Adjustments save');

                    const diff = this.summary().adjustment - this.form.value.adjustment!;
                    this.summary().total = this.summary().total - diff;
                    this.summary().remaining = this.summary().remaining - diff;

                    this.summary().adjustment = this.form.value.adjustment!;

                    this.submitting.set(false);

                    this.cd.detectChanges();
                },
                error: (e: HttpErrorResponse) => {
                    this.msgsSvc.isError('Unable to save adjustments');
                    this.submitting.set(false);
                },
            });
    }
}
