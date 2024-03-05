import { Component, DestroyRef, Input, OnInit } from "@angular/core";
import { FlashComponent } from "../../../components/flash/flash.component";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { CalendarComponent } from "../../../components/calendar/calendar.component";
import { startOfYear } from "date-fns";
import { PublicHolidaysService } from "../../../services/public-holidays/public-holidays.service";
import { PublicHolidayModel } from "../../../services/public-holidays/public-holiday.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { switchMap } from "rxjs";
import { ValidatorMessageComponent } from "../../../components/validator-message/validator-message.component";

@Component({
    standalone: true,
    templateUrl: 'public-holidays.component.html',
    providers: [PublicHolidaysService],
    imports: [FlashComponent, RouterLink, CalendarComponent, CommonModule, ReactiveFormsModule, ValidatorMessageComponent]
})
export class PublicHolidaysComponent implements OnInit {
    public companyName: string = '';

    public dateFormat: string = 'yyyy-mm-dd';

    public get currentYear() {
        return this.start.getFullYear();
    }

    public get nextYear() {
        return this.currentYear +1;
    }

    public get lastYear() {
        return this.currentYear -1;
    }

    public start = startOfYear(new Date());

    public holidays!: PublicHolidayModel[];

    public get holidaysForm() {
        return this.holidaySvc.holidays;
    }

    constructor(
        private readonly holidaySvc: PublicHolidaysService,
        private destroyed: DestroyRef,
        private readonly route: ActivatedRoute,
    ) {}

    public ngOnInit(): void {
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
            ).subscribe((data) => {
                this.holidaysForm.clear();

                data.map((h) => {
                    this.holidaySvc.holidays.push(this.holidaySvc.newForm(h,this.currentYear))
                });

                this.holidays = data;          
            });
    }

    public remove(id?: number | null) {
        this.holidaySvc.delete(id!)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe();
    }
}