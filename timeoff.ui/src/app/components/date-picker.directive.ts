import { AfterViewInit, Directive, ElementRef, EventEmitter, Optional, Output } from '@angular/core';
import { NgControl } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { formatISO, isValid, parseISO } from 'date-fns';

declare var $: any;

@Directive({
    standalone: true,
    selector: '[date-picker]',
})
export class DatePickerDirective implements AfterViewInit {
    @Output()
    public selected = new EventEmitter<Date>();

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
