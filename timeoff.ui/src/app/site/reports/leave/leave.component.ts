import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

import { FlashComponent } from "@components/flash/flash.component";
import { TeamSelectComponent } from "@components/team-select/team-select.component";
import { DatePickerDirective } from '@components/date-picker.directive';

import { LeaveResultModel } from './leave-result.model';

@Component({
    selector: 'leave',
    standalone: true,
    templateUrl: './leave.component.html',
    styleUrl: './leave.component.scss',
    imports: [RouterLink, FlashComponent, TeamSelectComponent,CommonModule, DatePickerDirective]
})
export class LeaveComponent {
    public results: LeaveResultModel[] = [];
}
