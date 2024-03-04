import { Component, Input, OnInit } from "@angular/core";
import { FlashComponent } from "../../../components/flash/flash.component";
import { RouterLink } from "@angular/router";
import { CalendarComponent } from "../../../components/calendar/calendar.component";
import { startOfYear } from "date-fns";

@Component({
    standalone: true,
    templateUrl: 'public-holidays.component.html',
    imports: [FlashComponent, RouterLink, CalendarComponent]
})
export class PublicHolidaysComponent {
    public companyName: string = '';

    @Input()
    public set year(value: number) {
        if (!!value) {
            this.start = startOfYear(value);
        } else {
            this.start = new Date();
        }
    }

    public get currentYear() {
        return this.start.getFullYear();
    }

    public get nextYear() {
        return this.currentYear +1;
    }

    public get lastYear() {
        return this.currentYear -1;
    }

    public start!: Date;

    constructor() {}
}