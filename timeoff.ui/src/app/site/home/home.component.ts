import { Component, OnInit } from "@angular/core";
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
export class HomeComponent implements OnInit{
    public name: string = '';

    public currentYear!: number;

    public get nextYear(): number { return this.currentYear +1; } 

    public get lastYear(): number { return this.currentYear -1; }

    public showFullYear: boolean = false;

    public allowanceSummary: AllowanceSummaryModel = new AllowanceSummaryModel();

    public managerName: string = 'manager';

    public managerEmail: string = 'manager@email';

    public teamName: string = 'team';

    public teamId: number = 0;

    public start: Date = new Date();

    constructor(private route: ActivatedRoute)
    {}

    public ngOnInit(): void {
        this.route.queryParamMap
            .subscribe((p) => {
                if (p.has('year')) {
                    this.currentYear = Number.parseInt(p.get('year')!);
                } else {
                    this.currentYear = new Date().getFullYear();
                }

                this.showFullYear =  !!p.get('showFullYear');

                this.start = this.showFullYear ? new Date(this.currentYear,0,1) : new Date();
            });
    }
}