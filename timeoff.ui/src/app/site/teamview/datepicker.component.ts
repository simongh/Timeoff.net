import { DatePipe } from "@angular/common";
import { AfterViewInit, Component, EventEmitter, Input, Output } from "@angular/core";

declare var $: any; 

@Component({
    standalone: true,
    templateUrl: 'datepicker.component.html',
    selector: 'date-picker',
    imports: [DatePipe]
})
export class DatePickerComponent implements AfterViewInit{
    private _date!: Date;

    public get date() {
        return this._date;
    }

    @Input()
    public set date(value: Date) {
        this._date = value;

        $('#team_view_month_select_btn').datepicker('update', value);
    }

    @Output()
    public selected = new EventEmitter<Date>();

    public ngAfterViewInit(): void {

        const setFn = (d: Date) => this.selected.emit(d);

        $('#team_view_month_select_btn')
            .datepicker()
            .on('changeDate', function(e: any) {
                setFn(e.date);
            });
    }
}