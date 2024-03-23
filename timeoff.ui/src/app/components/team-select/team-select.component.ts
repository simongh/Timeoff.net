import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CompanyService } from '../../services/company/company.service';
import { TeamModel } from '../../services/company/team.model';

@Component({
    selector: 'team-select',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule],
    templateUrl: './team-select.component.html',
    styleUrl: './team-select.component.sass',
    providers: [CompanyService],
})
export class TeamSelectComponent implements OnInit {
    public teams: TeamModel[] = [];

    @Input()
    public control!: FormControl<number | null>;

    @Input()
    public for: string = '';

    constructor(private destroyed: DestroyRef, private readonly companySvc: CompanyService) {}

    public ngOnInit(): void {
        this.companySvc
            .getTeams()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((teams) => (this.teams = teams));
    }
}
