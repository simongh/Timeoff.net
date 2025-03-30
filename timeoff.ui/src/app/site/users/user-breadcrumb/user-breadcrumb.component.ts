import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'user-breadcrumb',
    imports: [RouterLink],
    templateUrl: './user-breadcrumb.component.html',
    styleUrl: './user-breadcrumb.component.scss'
})
export class UserBreadcrumbComponent {
    public readonly name = input.required<string>();
}
