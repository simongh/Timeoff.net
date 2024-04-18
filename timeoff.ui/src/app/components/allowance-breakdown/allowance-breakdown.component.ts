import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';

import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';

@Component({
    selector: 'allowance-breakdown',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './allowance-breakdown.component.html',
    styleUrl: './allowance-breakdown.component.scss',
})
export class AllowanceBreakdownComponent {
    public summary = input.required<AllowanceSummaryModel>();
}
