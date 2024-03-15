import { Component, DestroyRef, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UsersService } from '../../../services/users/users.service';
import { ReactiveFormsModule } from '@angular/forms';
import { FlashComponent } from '../../../components/flash/flash.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TeamModel } from '../../../models/team.model';
import { CommonModule } from '@angular/common';
import { ValidatorMessageComponent } from '../../../components/validator-message/validator-message.component';
import { DatePickerDirective } from '../../../components/date-picker.directive';
import { HttpErrorResponse } from '@angular/common/http';
import { MessagesService } from '../../../services/messages/messages.service';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';

@Component({
    selector: 'user-create',
    standalone: true,
    providers: [UsersService],
    templateUrl: './user-create.component.html',
    styleUrl: './user-create.component.sass',
    imports: [
        RouterLink,
        ReactiveFormsModule,
        FlashComponent,
        CommonModule,
        ValidatorMessageComponent,
        DatePickerDirective,
        UserBreadcrumbComponent,
    ],
})
export class UserCreateComponent implements OnInit {
    public get form() {
        return this.usersSvc.form;
    }

    public dateFormat = 'yyyy-mm-dd';

    public companyName = '';

    public teams: TeamModel[] = [];

    constructor(
        private readonly usersSvc: UsersService,
        private readonly messagesSvc: MessagesService,
        private readonly router: Router,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        this.usersSvc
            .getTeams()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((teams) => (this.teams = teams));
    }

    public add() {
        this.form.markAllAsTouched();

        if (this.form.invalid) {
            return;
        }

        this.usersSvc
            .addUser()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.messagesSvc.isSuccess(
                        'New user account successfully added'
                    );
                    this.router.navigate(['users']);
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.messagesSvc.hasErrors(e.error.errors);
                    } else {
                        this.messagesSvc.isError('Unable to add new user');
                    }
                },
            });
    }
}
