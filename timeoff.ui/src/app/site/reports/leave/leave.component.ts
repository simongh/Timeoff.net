import { CommonModule } from '@angular/common';
import { Component, DestroyRef, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { combineLatest } from 'rxjs';
import { TippyDirective } from '@ngneat/helipopper';

import { FlashComponent } from '@components/flash/flash.component';
import { TeamSelectComponent } from '@components/team-select/team-select.component';
import { DateInputDirective } from '@components/date-input.directive';

import { CompanyService } from '@services/company/company.service';
import { LeaveTypeModel } from '@services/company/leave-type.model';

import { LeaveResultModel } from './leave-result.model';
import { LeaveService } from './leave.service';

@Component({
    selector: 'leave',
    templateUrl: './leave.component.html',
    styleUrl: './leave.component.scss',
    imports: [
        RouterLink,
        FlashComponent,
        TeamSelectComponent,
        CommonModule,
        DateInputDirective,
        ReactiveFormsModule,
        TippyDirective,
    ],
    providers: [CompanyService, LeaveService]
})
export class LeaveComponent implements OnInit {
    protected readonly results = signal<LeaveResultModel[]>([]);

    protected readonly leaveTypes = signal<LeaveTypeModel[]>([]);

    protected get form() {
        return this.leaveSvc.form;
    }

    protected readonly submitting = signal(false);

    constructor(
        private readonly companySvc: CompanyService,
        private readonly leaveSvc: LeaveService,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        combineLatest([this.companySvc.getLeaveTypes(), this.leaveSvc.search()])
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe(([data, results]) => {
                this.leaveTypes.set(data);
                this.results.set(results);
            });
    }

    protected search() {
        this.submitting.set(true);

        this.leaveSvc
            .search()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((l) => {
                this.results.set(l);
                this.submitting.set(false);
            });
    }
}
