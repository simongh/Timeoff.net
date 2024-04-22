import { CommonModule } from '@angular/common';
import { Component, DestroyRef, EventEmitter, Output, input, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';

import { FlashComponent } from '@components/flash/flash.component';

import { MessagesService } from '@services/messages/messages.service';

import { UsersService } from '../users.service';

@Component({
    selector: 'user-details',
    standalone: true,
    templateUrl: './user-details.component.html',
    styleUrl: './user-details.component.scss',
    imports: [CommonModule, RouterLink, FlashComponent, RouterLinkActive],
})
export class UserDetailsComponent {
    public readonly name = input.required<string>();

    public readonly isActive = input.required<boolean>();

    public readonly id = input.required<number>();

    @Output()
    public deleting = new EventEmitter();

    protected readonly submitting = signal(false);

    constructor(
        private destroyed: DestroyRef,
        private readonly usersSvc: UsersService,
        private readonly router: Router,
        private readonly msgsSvc: MessagesService
    ) {}

    public delete() {
        this.submitting.set(true);
        this.usersSvc
            .deleteUser(this.id())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.msgsSvc.isSuccess('Employee data removed', true);
                    this.submitting.set(false);

                    this.router.navigateByUrl('/users');
                },
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    }
                    this.msgsSvc.isError('Unable to remove employee');
                    this.submitting.set(false);
                },
            });
    }
}
