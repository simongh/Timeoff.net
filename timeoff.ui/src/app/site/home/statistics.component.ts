import { Component, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';
import { AllowanceBreakdownComponent } from '../../components/allowance-breakdown/allowance-breakdown.component';
import { LeaveSummaryComponent } from '../../components/leave-summary/leave-summary.component';

@Component({
    templateUrl: 'statistics.component.html',
    selector: 'statistics',
    imports: [RouterLink, AllowanceBreakdownComponent, LeaveSummaryComponent],
})
export class StatisticsComponent {
    public readonly year = input.required<number>();

    public readonly allowanceSummary = input.required<AllowanceSummaryModel>();

    protected readonly managerName = signal('manager');

    protected readonly managerEmail = signal('manager@email');

    protected readonly teamName = signal('team');

    protected readonly teamId = signal(0);
}
