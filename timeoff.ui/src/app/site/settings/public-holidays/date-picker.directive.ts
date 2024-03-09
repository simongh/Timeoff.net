import { formatDate } from "@angular/common";
import { AfterViewInit, Directive, ElementRef, Optional } from "@angular/core";
import { NgControl } from "@angular/forms";

declare var $: any; 

@Directive({
    standalone: true,
    selector: '[date-picker]'
})
export class DatePickerDirective implements AfterViewInit {
    constructor(
        private elRef: ElementRef, 
        @Optional()private ngControl: NgControl
    ) {}
    
    public ngAfterViewInit(): void {
        const setFn = (d: Date) =>{
            this.ngControl.control?.setValue(formatDate(d,'yyyy-MM-dd','en'));
            }

        $(this.elRef.nativeElement)
            .datepicker()
            .on('changeDate', function(e: any) {
                setFn(e.date);
            });
    }
}