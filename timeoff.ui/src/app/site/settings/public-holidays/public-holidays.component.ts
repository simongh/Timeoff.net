import { Component, DestroyRef, computed, numberAttribute, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { switchMap } from 'rxjs';
import { startOfYear } from 'date-fns';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { FlashComponent } from '@components/flash/flash.component';
import { CalendarComponent } from '@components/calendar/calendar.component';
import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';
import { DatePickerDirective } from '@components/date-picker.directive';

import { MessagesService } from '@services/messages/messages.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { PublicHolidayModel } from '@models/public-holiday.model';

import { PublicHolidaysService } from './public-holidays.service';
import { AddNewModalComponent } from './add-new-modal.component';

@Component({
    standalone: true,
    templateUrl: 'public-holidays.component.html',
    providers: [PublicHolidaysService],
    imports: [
        FlashComponent,
        RouterLink,
        CalendarComponent,
        CommonModule,
        ReactiveFormsModule,
        ValidatorMessageComponent,
        AddNewModalComponent,
        DatePickerDirective,
    ],
})
export class PublicHolidaysComponent {
    protected readonly companyName = this.currentUser.companyName;

    protected readonly dateFormat = this.currentUser.dateFormat;

    protected readonly currentYear = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    protected readonly nextYear = computed(() => this.currentYear() + 1);

    protected readonly lastYear = computed(() => this.currentYear() - 1);

    protected readonly start = computed(() => {
        this.getHolidays();
        const d = startOfYear(new Date());
        d.setFullYear(this.currentYear());
        return d;
    });

    protected readonly holidays = signal<PublicHolidayModel[]>([]);

    protected get holidaysForm() {
        return this.holidaySvc.holidays;
    }

    constructor(
        private readonly holidaySvc: PublicHolidaysService,
        private readonly msgsSvc: MessagesService,
        private readonly currentUser: LoggedInUserService,
        private destroyed: DestroyRef
    ) {}

    public remove(id?: number | null) {
        if (!id) {
            return;
        }

        this.holidaySvc
            .delete(id!)
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(() => {
                    return this.holidaySvc.get(this.currentYear());
                })
            )
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                    this.msgsSvc.isSuccess('Holiday was successfully removed');
                },
            });
    }

    public save() {
        this.holidaySvc
            .update(this.holidaysForm.value as PublicHolidayModel[])
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(() => {
                    return this.holidaySvc.get(this.currentYear());
                })
            )
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                    this.msgsSvc.isSuccess('Holidays updated');
                },
            });
    }

    private getHolidays() {
        this.holidaySvc
            .get(this.currentYear())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                },
            });
    }

    public create() {
        this.getHolidays();
    }

    private loadHolidays(data: PublicHolidayModel[]) {
        this.holidaysForm.clear();
        this.holidaySvc.setAddForm(this.currentYear());

        data.map((h) => {
            this.holidaySvc.holidays.push(this.holidaySvc.newForm(h, this.currentYear()));
        });

        this.holidays.set(data);
    }
}
