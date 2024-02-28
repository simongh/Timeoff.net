import { Component, OnInit } from "@angular/core";
import { FlashComponent } from "../../../components/flash/flash.component";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { CalendarComponent } from "../../../components/calendar/calendar.component";
import { startOfYear } from "date-fns";

@Component({
    standalone: true,
    templateUrl: 'public-holidays.component.html',
    imports: [FlashComponent, RouterLink, CalendarComponent]
})
export class PublicHolidaysComponent implements OnInit{
    public companyName: string = '';

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

    constructor(private route: ActivatedRoute)
    {}
    
    public ngOnInit(): void {
        this.route.queryParamMap
            .subscribe((p) => {
                if (p.has('year')) {
                    this.start = startOfYear(new Date(Number.parseInt(p.get('year')!),0,1));
                } else {
                    this.start = startOfYear(new Date());
                }
            });
    }
}