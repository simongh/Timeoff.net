import { CommonModule, WeekDay } from '@angular/common';
import { Component, Input } from '@angular/core';
import { DayModel } from './day.model';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'schedule',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './schedule.component.html',
  styleUrl: './schedule.component.sass'
})
export class ScheduleComponent {
  @Input()
  public days!: FormControl<boolean>[];

  public get models() {
    return this.days.map((d, i) => ({
      name: WeekDay[(i + 1) % 7].toString(),
      displayName: WeekDay[(i + 1) % 7].toString().substring(0,3),
      active: d.value,
      ctrl: d
    } as DayModel))
  }
}
