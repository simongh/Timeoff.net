import { Component, DestroyRef, OnInit } from '@angular/core';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { ActivatedRoute } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs';
import { UsersService } from '../../../services/users/users.service';

@Component({
    selector: 'app-user-schedule',
    standalone: true,
    templateUrl: './user-schedule.component.html',
    styleUrl: './user-schedule.component.sass',
    imports: [UserDetailsComponent, UserBreadcrumbComponent],
    providers: [UsersService]
})
export class UserScheduleComponent implements OnInit {
    public id!: number;

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService
    ) {}

    public ngOnInit(): void {
        this.route.paramMap
            .pipe(
                takeUntilDestroyed(this.destroyed),
                switchMap((p) => {
                    this.id = Number.parseInt(p.get('id')!);

                    return this.usersSvc.getUser(this.id);
                })
            )
            .subscribe(() => {});
    }
}
