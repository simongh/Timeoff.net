import { Component, inject, input, output } from '@angular/core';
import { EmailModel } from './email.model';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';
import { CommonModule } from '@angular/common';

@Component({
    templateUrl: 'results.component.html',
    selector: 'results',
    imports: [CommonModule],
})
export class ResultsComponent {
    protected readonly dateFormat = inject(LoggedInUserService).dateFormat;

    public readonly emails = input.required<EmailModel[]>();

    public userSelected = output<number>();

    public searchByUser(id: number) {
        this.userSelected.emit(id);
    }
}
