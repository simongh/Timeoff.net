import { AfterViewInit, Directive, ElementRef, Optional, output } from '@angular/core';
import { NgControl } from '@angular/forms';
import { formatISO, isValid, parseISO } from 'date-fns';

declare let $: any;

@Directive({
    standalone: true,
    selector: '[datePicker]',
})
export class DatePickerDirective implements AfterViewInit {
    public selected = output<Date>();

    constructor(private elRef: ElementRef, @Optional() private ngControl: NgControl) {}

    public ngAfterViewInit(): void {
        const setFn = (d: Date | null) => {
            if (!d) {
                this.ngControl?.control?.setValue(null);
                return;
            }
            this.ngControl?.control?.setValue(formatISO(d, { representation: 'date' }));

            this.selected.emit(d);
        };

        this.ngControl?.valueChanges?.subscribe((d) => {
            const valueDate = d ? parseISO(d) : null;
            const value = isValid(valueDate) ? valueDate : null;
            $(this.elRef.nativeElement).datepicker('update', value);
            //console.log('vc', d);
        });

        $(this.elRef.nativeElement)
            .datepicker()
            .on('changeDate', function (e: any) {
                setFn(e.date);
            })
            .on('clearDate', function (e: any) {
                setFn(null);
            });
    }
}
