import { Component, DestroyRef, OnInit } from '@angular/core';
import { FlashComponent } from '../../components/flash/flash.component';
import { AllowanceSummaryModel } from './allowance-summary.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CalendarComponent } from '../../components/calendar/calendar.component';
import { startOfYear } from 'date-fns';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
    standalone: true,
    templateUrl: 'home.component.html',
    imports: [FlashComponent, CommonModule, RouterLink, CalendarComponent],
})
export class HomeComponent implements OnInit {
    public name: string = '';

    public get year(): number {
        return this.start.getFullYear();
    }

    public get nextYear(): number {
        return this.start.getFullYear() + 1;
    }

    public get lastYear(): number {
        return this.start.getFullYear() - 1;
    }

    public showFullYear: boolean = false;

    public allowanceSummary = new AllowanceSummaryModel();

    public managerName: string = 'manager';

    public managerEmail: string = 'manager@email';

    public teamName: string = 'team';

    public teamId: number = 0;

    public start!: Date;

    constructor(
        private readonly route: ActivatedRoute,
        private readonly destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        this.route.queryParamMap
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((p) => {
                if (p.has('year')) {
                    this.start = startOfYear(Number.parseInt(p.get('year')!));
                } else {
                    this.start = startOfYear(new Date());
                }

                this.showFullYear = p.has('showFullYear');
            });
    }
}
