import { Component, Input } from '@angular/core';
import { LeaveSummary } from '../../services/calendar/leave-summary.model';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'leave-summary',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './leave-summary.component.html',
    styleUrl: './leave-summary.component.sass',
})
export class LeaveSummaryComponent {
    @Input()
    public items: LeaveSummary[] = [];
}
