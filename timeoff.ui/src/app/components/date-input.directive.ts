import {
    AfterViewInit,
    computed,
    Directive,
    effect,
    ElementRef,
    inject,
    input,
    OnDestroy,
    output,
} from '@angular/core';
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
    public readonly date = input<dateString | Date>();

    public readonly dateFormat = input<string>('YYYY-MM-DD');

    public readonly dateUpdated = output<dateString | null>();

    public readonly maxDate = input<dateString | null | undefined>(null);

    public readonly minDate = input<dateString | null | undefined>(null);

    public readonly showReset = input(false);

    private readonly el = inject(ElementRef);

    private readonly control = inject(NgControl, { optional: true });

    private instance!: easepick.Core;

    private readonly minDateAdjusted = computed(() => {
        const d = this.minDate();
        return d == null ? undefined : parseISO(d)!;
    });

    private readonly maxDateAdjusted = computed(() => {
        const d = this.maxDate();
        return d == null ? undefined : parseISO(d)!;
    });

    constructor() {
        this.init();

        effect(() => {
            const date = this.date();
            if (date) {
                this.instance.setDate(date);
                this.instance.gotoDate(date);
            } else {
                this.instance.clear();
            }
        });

        effect(() => {
            this.instance.options.LockPlugin!.maxDate = this.maxDateAdjusted();
            this.instance.PluginManager.reloadInstance('LockPlugin');
            this.instance.renderAll();
        });

        effect(() => {
            this.instance.options.LockPlugin!.minDate = this.minDateAdjusted();
            this.instance.PluginManager.reloadInstance('LockPlugin');
            this.instance.renderAll();
        });
    }

    ngAfterViewInit(): void {
        this.control?.valueChanges?.subscribe((v) => {
console.log(v);
            if (v) {
                this.instance.setDate(v);
                this.instance.gotoDate(v);
            } else if (!!this.instance.getDate()) {
                this.instance.clear();
            }
        });
    }

    ngOnDestroy(): void {
        this.instance.destroy();
    }

    private init() {
        this.instance = new easepick.create({
            element: this.el.nativeElement,
            css: ['https://cdn.jsdelivr.net/npm/@easepick/bundle@1.2.0/dist/index.css'],
            zIndex: 20,
            format: this.dateFormat(),
            AmpPlugin: {
                dropdown: {
                    months: true,
                    years: true,
                },
                darkMode: false,
                resetButton: this.showReset(),
            },
            LockPlugin: {
                minDate: this.minDateAdjusted(),
                maxDate: this.maxDateAdjusted(),
            },
            plugins: [AmpPlugin, LockPlugin],
            setup: (picker) => {
                picker.on('select', (evt) => {
                    const date = formatISO(picker.getDate(), { representation: 'date' });
                    this.control?.control?.setValue(date);
                    this.dateUpdated.emit(date);
                });
                picker.on('clear', () => {
                    this.control?.control?.setValue(null);
                    this.dateUpdated.emit(null);
                });
            },
        });

        this.instance;
    }
}
