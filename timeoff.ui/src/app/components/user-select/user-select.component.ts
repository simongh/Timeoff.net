import { Component, DestroyRef, effect, input, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { CompanyService } from '@services/company/company.service';
import { UserModel } from '@services/company/user.model';

@Component({
    selector: 'user-select',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './user-select.component.html',
    styleUrl: './user-select.component.scss',
    providers: [CompanyService],
})
export class UserListComponent {
    protected readonly users = signal<UserModel[]>([]);

    public for = input('');

    public control = input.required<FormControl<number | null>>();

    constructor(private destroyed: DestroyRef, private readonly companySvc: CompanyService) {
        effect(() => {
            this.companySvc
                .getUsers()
                .pipe(takeUntilDestroyed(this.destroyed))
                .subscribe((users) => this.users.set(users));
        });
    }
}
