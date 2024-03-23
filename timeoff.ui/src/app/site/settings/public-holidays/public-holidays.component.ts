import { Component, DestroyRef, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { switchMap } from 'rxjs';
import { startOfYear } from 'date-fns';
import { FlashComponent } from '../../../components/flash/flash.component';
import { CalendarComponent } from '../../../components/calendar/calendar.component';
import { PublicHolidaysService } from '../../../services/public-holidays/public-holidays.service';
import { PublicHolidayModel } from '../../../services/public-holidays/public-holiday.model';
import { ValidatorMessageComponent } from '../../../components/validator-message/validator-message.component';
import { AddNewModalComponent } from './add-new-modal.component';
import { DatePickerDirective } from '../../../components/date-picker.directive';
import { MessagesService } from '../../../services/messages/messages.service';

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
export class PublicHolidaysComponent implements OnInit {
    public companyName: string = '';

    public dateFormat: string = 'yyyy-mm-dd';

    public get currentYear() {
        return this.start.getFullYear();
    }

    public get nextYear() {
        return this.currentYear + 1;
    }

    public get lastYear() {
        return this.currentYear - 1;
    }

    public start = startOfYear(new Date());

    public holidays!: PublicHolidayModel[];

    public get holidaysForm() {
        return this.holidaySvc.holidays;
    }

    constructor(
        private readonly holidaySvc: PublicHolidaysService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef,
        private readonly route: ActivatedRoute
    ) {}

    public ngOnInit(): void {
        this.getHolidays();
    }

    public remove(id?: number | null) {
        if (!id) {
            return;
        }

        this.holidaySvc
            .delete(id!)
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(() => {
                    return this.holidaySvc.get(this.currentYear);
                })
            )
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                    this.msgsSvc.isSuccess('Holiday was successfully removed');
                },
                error: (error: HttpErrorResponse) => {
                    if (error.status == 400) {
                        this.msgsSvc.hasErrors(error.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to remove holiday');
                    }
                },
            });
    }

    public save() {
        this.holidaySvc
            .update(this.holidaysForm.value as PublicHolidayModel[])
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(() => {
                    return this.holidaySvc.get(this.currentYear);
                })
            )
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                    this.msgsSvc.isSuccess('Holidays updated');
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to update holidays');
                    }
                },
            });
    }

    public getHolidays() {
        this.route.queryParamMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((r) => {
                    if (r.has('year')) {
                        this.start.setFullYear(Number.parseInt(r.get('year')!));
                    } else {
                        this.start = startOfYear(new Date());
                    }

                    return this.holidaySvc.get(this.currentYear);
                })
            )
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                },
            });
    }

    public create() {
        this.holidaySvc
            .get(this.currentYear)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => {
                    this.loadHolidays(data);
                },
                error: (e: HttpErrorResponse) => {},
            });
    }

    private loadHolidays(data: PublicHolidayModel[]) {
        this.holidaysForm.clear();

        data.map((h) => {
            this.holidaySvc.holidays.push(this.holidaySvc.newForm(h, this.currentYear));
        });

        this.holidays = data;
    }
}
