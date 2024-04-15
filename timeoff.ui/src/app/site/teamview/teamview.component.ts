import { CommonModule } from '@angular/common';
import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { addMonths, startOfMonth, subMonths } from 'date-fns';
import { combineLatest, map, switchMap } from 'rxjs';

import { DatePickerDirective } from '@components/date-picker.directive';

import { TeamModel } from '@services/company/team.model';
import { CompanyService } from '@services/company/company.service';

import { MonthViewComponent } from './month-view.component';
import { TeamViewService } from './team-view.service';
import { TeamViewModel } from './team-view.model';
import { UserSummaryModel } from './user-summary.model';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    selector: 'team-view',
    standalone: true,
    templateUrl: './teamview.component.html',
    styleUrl: './teamview.component.scss',
    imports: [CommonModule, RouterLink, MonthViewComponent, DatePickerDirective],
    providers: [CompanyService, TeamViewService],
})
export class TeamviewComponent implements OnInit {
    public get name() {
        return this.currentUser.userName;
    }

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

    public results: TeamViewModel = {
        users: [] as UserSummaryModel[]
    } as TeamViewModel;

    constructor(
        private readonly route: ActivatedRoute,
        private readonly destroyed: DestroyRef,
        private readonly router: Router,
        private readonly companySvc: CompanyService,
        private readonly teamViewSvc: TeamViewService,
        private readonly currentUser: LoggedInUserService,
    ) {
        this.setStart(startOfMonth(new Date()));
    }

    public ngOnInit(): void {
        combineLatest([this.route.queryParamMap, this.companySvc.getTeams()])
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap(([p, teams]) => {
                    if (p.has('year') && p.has('month')) {
                        this.setStart(new Date(`${p.get('year')}-${p.get('month')}-01`));
                    }

                    if (p.has('team')) {
                        this.team = Number.parseInt(p.get('team')!);
                    } else {
                        this.team = null;
                    }

                    return this.teamViewSvc.getSummaryForUsers(this.start, this.team).pipe(
                        map((results) => ({
                            teams: teams,
                            results: results,
                            grouped: p.has('grouped')
                        }))
                    );
                })
            )
            .subscribe((data) => {
                this.teams = data.teams;
                this.grouped = data.grouped;
                this.results = data.results;
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
