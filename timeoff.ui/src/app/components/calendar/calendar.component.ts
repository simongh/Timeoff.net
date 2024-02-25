import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { addMonths, eachDayOfInterval, eachMonthOfInterval, eachWeekOfInterval, endOfMonth, endOfWeek, getDay} from "date-fns";

@Component({
    selector: 'calendar',
    standalone: true,
    templateUrl: 'calendar.component.html',
    imports: [CommonModule]
})
export class CalendarComponent {
    @Input()
    public monthCount: number = 0;

    @Input()
    public monthFormat: string = ""

    @Input()
    public start: Date = new Date();

    public get weeks() {
        return eachMonthOfInterval({
            start: this.start,
            end: addMonths(this.start, this.monthCount -1) 
         }).map((m) => ({
            date: m,
            padding: (getDay(m) + 6) % 7,
            weeks: eachWeekOfInterval({
                start: m,
                end: endOfMonth(m)
            }, {weekStartsOn:1}).map((w,i) => ({
                num: i,
                days: eachDayOfInterval({
                    start: w,
                    end: endOfWeek(w, {weekStartsOn:1})
                }).filter((day) => day.getMonth() == m.getMonth())
            }))
        }));
    }
}