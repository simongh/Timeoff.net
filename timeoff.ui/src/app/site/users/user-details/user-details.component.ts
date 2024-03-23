import { CommonModule } from '@angular/common';
import { Component, DestroyRef, EventEmitter, Input, Output } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { FlashComponent } from '../../../components/flash/flash.component';
import { UsersService } from '../../../services/users/users.service';
import { MessagesService } from '../../../services/messages/messages.service';

@Component({
    selector: 'user-details',
    standalone: true,
    templateUrl: './user-details.component.html',
    styleUrl: './user-details.component.sass',
    imports: [CommonModule, RouterLink, FlashComponent, RouterLinkActive],
})
export class UserDetailsComponent {
    @Input()
    public name!: string;

    @Input()
    public isActive!: boolean;

    @Input()
    public id!: number;

    @Output()
    public deleting = new EventEmitter();

    public submitting = false;

    constructor(
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly router: Router,
        private readonly msgsSvc: MessagesService
    ) {}

    public delete() {
        this.submitting = true;
        this.usersSvc
            .deleteUser(this.id)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Employee data removed', true);
                    this.submitting = false;

                    this.router.navigate(['users']);
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    }
                    this.msgsSvc.isError('Unable to remove employee');
                    this.submitting = false;
                },
            });
    }
}
