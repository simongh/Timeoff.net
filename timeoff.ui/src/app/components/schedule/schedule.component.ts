import { CommonModule, WeekDay } from '@angular/common';
import { Component, Input, computed, input } from '@angular/core';
import { DayModel } from './day.model';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
    selector: 'schedule',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './schedule.component.html',
    styleUrl: './schedule.component.scss',
})
export class ScheduleComponent {
    public days = input.required<FormControl<boolean | null>[]>();

    protected readonly models = computed(()=> {
        return this.days().map(
            (d, i) =>
                ({
                    name: WeekDay[(i + 1) % 7].toString(),
                    displayName: WeekDay[(i + 1) % 7].toString().substring(0, 3),
                } as DayModel)
        );
    });

    protected change(index: number) {
        this.days()[index].setValue(!this.days()[index].value);
    }
}
