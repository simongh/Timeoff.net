import { Component, DestroyRef, computed, inject, numberAttribute, signal } from '@angular/core';
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
import { DateInputDirective } from '@components/date-input.directive';

import { MessagesService } from '@services/messages/messages.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { PublicHolidayModel } from '@models/public-holiday.model';
import { CalendarDayModel } from '@models/calendar-day.model';

import { PublicHolidaysService } from './public-holidays.service';
import { AddNewModalComponent } from './add-new-modal.component';

@Component({
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
        DateInputDirective,
    ],
})
export class PublicHolidaysComponent {
    readonly #destroyed = inject(DestroyRef);

    readonly #holidaySvc = inject(PublicHolidaysService);

    readonly #messagesSvc = inject(MessagesService);

    protected readonly companyName = inject(LoggedInUserService).companyName;

    protected readonly dateFormat = inject(LoggedInUserService).dateFormat;

    protected readonly currentYear = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    protected readonly nextYear = computed(() => this.currentYear() + 1);

    protected readonly lastYear = computed(() => this.currentYear() - 1);

    protected readonly start = computed(() => {
        this.getHolidays();
        const d = startOfYear(new Date());
        d.setFullYear(this.currentYear());
        return d;
    });

    protected readonly holidays = signal<CalendarDayModel[]>([]);

    protected get holidaysForm() {
        return this.#holidaySvc.holidays;
    }

    constructor() {
        this.#holidaySvc.setAddForm(this.currentYear());
    }

    public remove(id?: number | null) {
        if (!id) {
            return;
        }

        this.#holidaySvc
            .delete(id!)
            .pipe(
                takeUntilDestroyed(this.#destroyed),
                switchMap(() => {
                    return this.#holidaySvc.get(this.currentYear());
                })
            )
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                    this.#messagesSvc.isSuccess('Holiday was successfully removed');
                },
            });
    }

    public save() {
        this.#holidaySvc
            .update(this.holidaysForm.value as PublicHolidayModel[])
            .pipe(
                takeUntilDestroyed(this.#destroyed),
                switchMap(() => {
                    return this.#holidaySvc.get(this.currentYear());
                })
            )
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                    this.#messagesSvc.isSuccess('Holidays updated');
                },
            });
    }

    private getHolidays() {
        this.#holidaySvc
            .get(this.currentYear())
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                },
            });
    }

    public create() {
        this.getHolidays();
    }

    private loadHolidays(data: CalendarDayModel[]) {
        this.holidaysForm.clear();
        this.#holidaySvc.setAddForm(this.currentYear());

        data.map((h) => {
            this.#holidaySvc.holidays.push(this.#holidaySvc.newForm(h, this.currentYear()));
        });

        this.holidays.set(data);
    }
}
