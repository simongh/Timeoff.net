import { Component, input } from '@angular/core';
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
    public requests = input<LeaveRequestModel[]>([]);

    public dateFormat = input('yyyy-MM-dd');
}
