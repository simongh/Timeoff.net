import { AfterViewInit, Directive, effect, ElementRef, inject, input, OnDestroy, output } from '@angular/core';
import { NgControl } from '@angular/forms';
import { easepick } from '@easepick/core';
import { AmpPlugin } from '@easepick/amp-plugin';
import { dateString } from '@models/types';
import { LockPlugin } from '@easepick/lock-plugin';
import { formatISO, parseISO } from 'date-fns';

@Directive({
    selector: '[dateInput]',
    standalone: true,
})
export class DateInputDirective implements OnDestroy, AfterViewInit {
    public date = input<dateString>();

    public dateFormat = input<string>();

    public dateUpdated = output<dateString>();

    public maxDate = input(undefined, {
        transform: (v: dateString | null | undefined) => (v == null ? undefined : v),
    });

    public minDate = input(undefined, {
        transform: (v: dateString | null | undefined) => (v == null ? undefined : v),
    });

    private el = inject(ElementRef);

    private control = inject(NgControl, { optional: true });

    private instance!: easepick.Core;

    constructor() {
        effect(() => {
            const date = this.date();
            if (date) {
                this.instance.setDate(date);
                this.instance.gotoDate(date);
            } else {
                this.instance.clear();
            }
        });

        effect(()=>{
            const maxDate = this.maxDate();

            this.instance.options.LockPlugin!.maxDate=maxDate ? parseISO(maxDate) : undefined;
            this.instance.PluginManager.reloadInstance('LockPlugin');
        });

        effect(()=>{
            const minDate = this.minDate();

            this.instance.options.LockPlugin!.minDate = minDate? parseISO(minDate) : undefined;
            this.instance.PluginManager.reloadInstance('LockPlugin');
        })
    }

    ngAfterViewInit(): void {
        this.control?.valueChanges?.subscribe((v) => {
            //console.log('vc', v);
            if (v) {
                this.instance.setDate(v);
                this.instance.gotoDate(v);
            } else {
                this.instance.clear();
            }
        });

        this.init();
    }

    ngOnDestroy(): void {
        this.instance.destroy();
    }

    private init() {
        this.instance = new easepick.create({
            element: this.el.nativeElement,
            css: ['https://cdn.jsdelivr.net/npm/@easepick/bundle@1.2.0/dist/index.css'],
            zIndex: 20,
            AmpPlugin: {
                dropdown: {
                    months: true,
                    years: true,
                },
                darkMode: false,
            },
            LockPlugin: {
                minDate: this.minDate(),
                maxDate: this.maxDate(),
            },
            plugins: [AmpPlugin, LockPlugin],
            setup: (picker) => {
                picker.on('select', (evt) => {
                    const date = formatISO(picker.getDate(), { representation: 'date' });
                    //console.log('control', this.control?.value);
                    //console.log('picker', date);
                    this.control?.control?.setValue(date);

                    this.dateUpdated.emit(date);
                    //console.log('control after update', this.control?.value);
                });
            },
        });

        this.instance;
    }
}
