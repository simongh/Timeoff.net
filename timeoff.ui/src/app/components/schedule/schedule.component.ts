import { CommonModule, WeekDay } from '@angular/common';
import { Component, computed, input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { DayModel } from './day.model';
import { ScheduleFormGroup } from './schedule-form';

@Component({
    selector: 'schedule',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './schedule.component.html',
    styleUrl: './schedule.component.scss',
})
export class ScheduleComponent {
    public readonly days = input.required<ScheduleFormGroup>()

    protected readonly models = computed(()=> {
        const values = this.days();

        const models= Object.keys(values.value).map(
            (d, i) =>
                ({
                    name: WeekDay[(i + 1) % 7].toString(),
                    displayName: WeekDay[(i + 1) % 7].toString().substring(0, 3),
                    control: values.get(d),
                } as DayModel)
        );
        return models;
    });

    protected change(model: DayModel) {
        model.control.setValue(!model.control.value);
    }
}
