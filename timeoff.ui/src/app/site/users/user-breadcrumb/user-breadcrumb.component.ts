import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'user-breadcrumb',
    standalone: true,
    imports: [RouterLink],
    templateUrl: './user-breadcrumb.component.html',
    styleUrl: './user-breadcrumb.component.scss',
})
export class UserBreadcrumbComponent {
    @Input()
    public name!: string;
}
