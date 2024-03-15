import { Component, DestroyRef, OnInit } from '@angular/core';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { UsersService } from '../../../services/users/users.service';
import { combineLatest, switchMap } from 'rxjs';
import { endOfDay, endOfToday, formatDate, isAfter, isBefore, parseISO, startOfToday } from 'date-fns';
import { TeamModel } from '../../../models/team.model';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'user-edit',
    standalone: true,
    templateUrl: './user-edit.component.html',
    styleUrl: './user-edit.component.sass',
    imports: [UserDetailsComponent, RouterLink, UserBreadcrumbComponent,ReactiveFormsModule,CommonModule],
    providers: [UsersService],
})
export class UserEditComponent implements OnInit {
    public companyName = '';

    public dateFormat = 'yyyy-mm-dd';

    public id = 0;

    public get form() {
        return this.usersSvc.form;
    }

    public get fullName() {
        return `${this.form.controls.firstName.value} ${this.form.controls.lastName.value}`;
    }

    public get userEnabled(){
        const ending = this.form.controls.endDate.value;
        const isActive = this.form.controls.isActive.value;
        if (!!ending) {
            return isAfter(parseISO(ending),endOfToday()) && isActive;
        } else {
            return isActive;
        }
    }
    public teams: TeamModel[] = [];

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService
    ) {}

    public ngOnInit(): void {
        combineLatest([
            this.route.paramMap.pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
                    this.id = Number.parseInt(p.get('id')!);

                    return this.usersSvc.getUser(this.id);
                })
            ),
            this.usersSvc.getTeams()
        ]).subscribe(([user, teams]) => {
            this.teams = teams;

            this.form.setValue({
                firstName: user.firstName,
                lastName: user.lastName,
                email: user.email,
                isAdmin: user.isAdmin,
                autoApprove: user.autoApprove,
                team: user.teamId,
                startDate: formatDate(user.startDate, 'yyyy-MM-dd'),
                endDate: user.endDate
                    ? formatDate(user.endDate, 'yyyy-MM-dd')
                    : null,
                isActive: user.isActive,
            });
        });
    }
}
