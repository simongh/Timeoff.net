import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';
import { TippyDirective } from '@ngneat/helipopper';

import { AllowanceSummaryModel } from '@services/calendar/allowance-summary.model';

@Component({
    selector: 'allowance-breakdown',
    imports: [CommonModule, TippyDirective],
    templateUrl: './allowance-breakdown.component.html',
    styleUrl: './allowance-breakdown.component.scss'
})
export class AllowanceBreakdownComponent {
    public summary = input.required<AllowanceSummaryModel>();
}
