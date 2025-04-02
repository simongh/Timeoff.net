import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TippyDirective } from '@ngneat/helipopper';

import { FlashComponent } from '@components/flash/flash.component';
import { YesPipe } from '@components/yes.pipe';

import { TeamsService } from '../teams.service';
import { TeamModel } from '../team.model';
import { AddNewModalComponent } from './add-new-modal.component';

@Component({
    templateUrl: 'teams-list.component.html',
    providers: [TeamsService],
    imports: [FlashComponent, CommonModule, RouterLink, YesPipe, AddNewModalComponent, TippyDirective]
})
export class TeamsListComponent implements OnInit {
    readonly #destroyed = inject(DestroyRef);
    readonly #teamSvc = inject(TeamsService);

    protected readonly teams = signal<TeamModel[]>([]);

    public ngOnInit(): void {
        this.refresh();
    }

    public added() {
        this.refresh();
    }

    private refresh() {
        this.#teamSvc
            .getTeams()
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe((t) => {
                this.teams.set(t);
            });
    }
}
