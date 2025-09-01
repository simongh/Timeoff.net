import { Component, input } from '@angular/core';

import { LeaveRequestModel } from '@models/leave-request.model';

import { DatePartPipe } from '../date-part.pipe';

@Component({
    selector: 'requests-list',
    standalone: true,
    templateUrl: './requests-list.component.html',
    styleUrl: './requests-list.component.scss',
    imports: [DatePartPipe],
})
export class RequestsListComponent {
    public readonly requests = input<LeaveRequestModel[]>([]);

    public readonly dateFormat = input('yyyy-MM-dd');
}
