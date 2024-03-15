import { CommonModule } from '@angular/common';
import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { addMonths, startOfMonth, subMonths } from 'date-fns';
import { TeamModel } from '../../models/team.model';
import { MonthViewComponent } from './month-view.component';
import { DatePickerDirective } from '../../components/date-picker.directive';

@Component({
    standalone: true,
    templateUrl: 'teamview.component.html',
    imports: [
        CommonModule,
        RouterLink,
        MonthViewComponent,
        DatePickerDirective
    ],
})
export class TeamviewComponent implements OnInit {
    public name: string = '';

    public get year() {
        return this.start.getFullYear();
    }

    public get month() {
        return this.start.getMonth() + 1;
    }

    public get lastParams() {
        return this.toParams(this.last);
    }

    public get nextParams() {
        return this.toParams(this.next);
    }

    public grouped: boolean = false;

    public team: number | null = null;

    public start!: Date;

    public last!: Date;

    public next!: Date;

    public teams: TeamModel[] = [{ id: 1, name: 'test' }];

    constructor(
        private route: ActivatedRoute,
        private destroyed: DestroyRef,
        private router: Router
    ) {}

    public ngOnInit(): void {
        this.route.queryParamMap
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((p) => {
                if (p.has('year') && p.has('month')) {
                    this.setStart(
                        new Date(`${p.get('year')}-${p.get('month')}-01`)
                    );
                } else {
                    this.setStart(startOfMonth(new Date()));
                }

                this.grouped = p.has('grouped');

                if (p.has('team')) {
                    this.team = Number.parseInt(p.get('team')!);
                } else {
                    this.team = null;
                }
            });
    }

    public dateselected(e: Date) {
        this.router.navigate([], {
            queryParams: this.toParams(e),
            relativeTo: this.route,
        });
    }

    private setStart(when: Date) {
        this.start = when;
        this.last = subMonths(this.start, 1);
        this.next = addMonths(this.start, 1);
    }

    private toParams(when: Date) {
        return {
            year: when.getFullYear(),
            month: when.getMonth() + 1,
            grouped: this.grouped ? true : null,
            team: this.team,
        };
    }
}
