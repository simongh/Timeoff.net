import { Component, DestroyRef } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { UsersService } from '../../../services/users/users.service';
import { FlashComponent } from '../../../components/flash/flash.component';
import { ValidatorMessageComponent } from '../../../components/validator-message/validator-message.component';
import { DatePickerDirective } from '../../../components/date-picker.directive';
import { MessagesService } from '../../../services/messages/messages.service';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';
import { TeamSelectComponent } from '../../../components/team-select/team-select.component';

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
        TeamSelectComponent,
    ],
})
export class UserCreateComponent {
    public get form() {
        return this.usersSvc.form;
    }

    public dateFormat = 'yyyy-mm-dd';

    public companyName = '';

    constructor(
        private readonly usersSvc: UsersService,
        private readonly messagesSvc: MessagesService,
        private readonly router: Router,
        private destroyed: DestroyRef
    ) {}

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
                    this.messagesSvc.isSuccess('New user account successfully added', true);
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
