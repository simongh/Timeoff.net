import { formatDate } from '@angular/common';
import { AfterViewInit, Directive, ElementRef, EventEmitter, Optional, Output } from '@angular/core';
import { NgControl } from '@angular/forms';

declare var $: any;

@Directive({
    standalone: true,
    selector: '[date-picker]',
})
export class DatePickerDirective implements AfterViewInit {
    @Output()
    public selected = new EventEmitter<Date>();

    constructor(
        private elRef: ElementRef,
        @Optional() private ngControl: NgControl
    ) {}

    public ngAfterViewInit(): void {
        const setFn = (d: Date) => {
            this.ngControl?.control?.setValue(formatDate(d, 'yyyy-MM-dd', 'en'));

            this.selected.emit(d);
        };

        $(this.elRef.nativeElement)
            .datepicker()
            .on('changeDate', function (e: any) {
                setFn(e.date);
            });
    }
}
