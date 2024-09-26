import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, booleanAttribute, computed, numberAttribute, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router, RouterLink } from '@angular/router';
import { addMonths, parseISO, subMonths } from 'date-fns';
import { combineLatest } from 'rxjs';
import { injectQueryParams } from 'ngxtension/inject-query-params';

import { DateInputDirective } from '@components/date-input.directive';

import { TeamModel } from '@services/company/team.model';
import { CompanyService } from '@services/company/company.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { MonthViewComponent } from './month-view.component';
import { TeamViewService } from './team-view.service';
import { UserSummaryModel } from './user-summary.model';
import { dateString } from '@models/types';

@Component({
    selector: 'team-view',
    standalone: true,
    templateUrl: './teamview.component.html',
    styleUrl: './teamview.component.scss',
    imports: [CommonModule, RouterLink, MonthViewComponent, DateInputDirective],
    providers: [CompanyService, TeamViewService],
})
export class TeamviewComponent implements OnInit {
    protected readonly name = this.currentUser.userName;

    protected readonly year = injectQueryParams((p) => numberAttribute(p['year'] ?? new Date().getFullYear()));

    protected readonly month = injectQueryParams((p) => numberAttribute(p['month'] ?? new Date().getMonth() + 1));

    protected readonly lastParams = computed(() => this.toParams(this.last()));

    protected readonly nextParams = computed(() => this.toParams(this.next()));

    protected readonly grouped = injectQueryParams((p) => booleanAttribute(p['grouped'] ?? false));

    protected readonly team = injectQueryParams((p) => {
        const team = p['team'];
        return team ? Number.parseInt(team) : null;
    });

    protected readonly start = computed(() => new Date(this.year(), this.month() - 1));

    protected readonly last = computed(() => subMonths(this.start(), 1));

    protected readonly next = computed(() => addMonths(this.start(), 1));

    protected readonly teams = signal<TeamModel[]>([]);

    protected readonly results = signal<UserSummaryModel[]>([]);

    constructor(
        private readonly destroyed: DestroyRef,
        private readonly router: Router,
        private readonly companySvc: CompanyService,
        private readonly teamViewSvc: TeamViewService,
        private readonly currentUser: LoggedInUserService
    ) {}

    public ngOnInit(): void {
        combineLatest([this.teamViewSvc.getSummaryForUsers(this.start(), this.team()), this.companySvc.getTeams()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([results, teams]) => {
                this.teams.set(teams);
                this.results.set(results);
            });
    }

    public dateselected(e: dateString | null) {
        if (!e) {
            return;
        }
        this.router.navigate(['/teamview'], {
            queryParams: this.toParams(parseISO(e)),
        });
    }

    private toParams(when: Date) {
        return {
            year: when.getFullYear(),
            month: when.getMonth() + 1,
            grouped: this.grouped() ? true : null,
            team: this.team(),
        };
    }
}
