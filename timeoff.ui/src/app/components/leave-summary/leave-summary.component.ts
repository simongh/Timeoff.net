import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LeaveSummary } from '@services/calendar/leave-summary.model';

@Component({
    selector: 'leave-summary',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './leave-summary.component.html',
    styleUrl: './leave-summary.component.scss',
})
export class LeaveSummaryComponent {
    @Input()
    public items: LeaveSummary[] = [];
}
