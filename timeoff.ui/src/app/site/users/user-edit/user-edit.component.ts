import { Component, DestroyRef, OnInit } from '@angular/core';
import { UserDetailsComponent } from '../user-details/user-details.component';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UserBreadcrumbComponent } from '../user-breadcrumb/user-breadcrumb.component';

@Component({
    selector: 'user-edit',
    standalone: true,
    templateUrl: './user-edit.component.html',
    styleUrl: './user-edit.component.sass',
    imports: [UserDetailsComponent, RouterLink, UserBreadcrumbComponent],
})
export class UserEditComponent implements OnInit {
    public companyName = '';

    public id = 0;

    constructor(
        private readonly route: ActivatedRoute,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        this.route.paramMap
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe((p) => {
                this.id = Number.parseInt(p.get('id')!);
            });
    }
}
