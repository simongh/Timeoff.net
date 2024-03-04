import { Component, Input, OnInit } from "@angular/core";
import { FlashComponent } from "../../components/flash/flash.component";
import { AllowanceSummaryModel } from "./allowance-summary.model";
import { CommonModule } from "@angular/common";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { CalendarComponent } from "../../components/calendar/calendar.component";
import { startOfYear } from "date-fns";

@Component({
    standalone: true,
    templateUrl: 'home.component.html',
    imports: [FlashComponent, CommonModule, RouterLink, CalendarComponent]
})
export class HomeComponent {
    public name: string = '';

    @Input()
    public set year(year: number | null) { 
        if (!!year) {
            this.start = startOfYear(year);
        } else {
            this.start = startOfYear(new Date());
        }
    }
    public get year(): number { return this.start.getFullYear(); }

    public get nextYear(): number { return this.start.getFullYear() +1; } 

    public get lastYear(): number { return this.start.getFullYear() -1; }

    @Input()
    public showFullYear: boolean = false;

    public allowanceSummary: AllowanceSummaryModel = new AllowanceSummaryModel();

    public managerName: string = 'manager';

    public managerEmail: string = 'manager@email';

    public teamName: string = 'team';

    public teamId: number = 0;

    public start!: Date;

    constructor() { }
}