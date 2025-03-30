import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LeaveSummary } from '@services/calendar/leave-summary.model';

@Component({
    selector: 'leave-summary',
    imports: [CommonModule],
    templateUrl: './leave-summary.component.html',
    styleUrl: './leave-summary.component.scss'
})
export class LeaveSummaryComponent {
    public readonly items = input<LeaveSummary[]>([]);
}
