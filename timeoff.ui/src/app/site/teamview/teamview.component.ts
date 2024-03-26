import { CommonModule } from '@angular/common';
import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { addMonths, startOfMonth, subMonths } from 'date-fns';
import { combineLatest } from 'rxjs';

import { DatePickerDirective } from '@components/date-picker.directive';

import { TeamModel } from '@services/company/team.model';
import { CompanyService } from '@services/company/company.service';

import { MonthViewComponent } from './month-view.component';

@Component({
    selector: 'team-view',
    standalone: true,
    templateUrl: './teamview.component.html',
    styleUrl: './teamview.component.scss',
    imports: [CommonModule, RouterLink, MonthViewComponent, DatePickerDirective],
    providers: [CompanyService],
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

    public teams: TeamModel[] = [];

    constructor(
        private route: ActivatedRoute,
        private destroyed: DestroyRef,
        private router: Router,
        private companySvc: CompanyService
    ) {}

    public ngOnInit(): void {
        combineLatest([this.route.queryParamMap, this.companySvc.getTeams()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([p, teams]) => {
                if (p.has('year') && p.has('month')) {
                    this.setStart(new Date(`${p.get('year')}-${p.get('month')}-01`));
                } else {
                    this.setStart(startOfMonth(new Date()));
                }

                this.grouped = p.has('grouped');

                this.teams = teams;

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
