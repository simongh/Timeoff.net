import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaveRequestModel } from '../../models/leave-request.model';
import { DatePartPipe } from '../date-part.pipe';

@Component({
    selector: 'requests-list',
    standalone: true,
    templateUrl: './requests-list.component.html',
    styleUrl: './requests-list.component.scss',
    imports: [CommonModule, DatePartPipe],
})
export class RequestsListComponent {
    @Input()
    public requests: LeaveRequestModel[] = [];

    public dateFormat = 'yyyy-MM-dd';
}
