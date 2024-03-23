import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { CompanyService } from '../../services/company/company.service';
import { UserModel } from '../../services/company/user.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'user-select',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule],
    templateUrl: './user-select.component.html',
    styleUrl: './user-select.component.sass',
    providers: [CompanyService],
})
export class UserListComponent implements OnInit {
    public users: UserModel[] = [];

    @Input()
    public for: string = '';

    @Input()
    public control!: FormControl<number | null>;

    constructor(private destroyed: DestroyRef, private readonly companySvc: CompanyService) {}

    public ngOnInit(): void {
        this.companySvc
            .getUsers()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((users) => (this.users = users));
    }
}
