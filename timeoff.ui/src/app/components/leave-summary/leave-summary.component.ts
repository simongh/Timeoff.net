import { Component, input } from '@angular/core';

import { LeaveSummary } from '@services/calendar/leave-summary.model';

@Component({
    selector: 'leave-summary',
    standalone: true,
    imports: [],
    templateUrl: './leave-summary.component.html',
    styleUrl: './leave-summary.component.scss',
})
export class LeaveSummaryComponent {
    public readonly items = input<LeaveSummary[]>([]);
}
