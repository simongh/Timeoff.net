import { Component, input } from '@angular/core';

import { LeaveRequestModel } from '@models/leave-request.model';

@Component({
    templateUrl: 'absences.component.html',
    selector: 'absences',
})
export class AbsencesComponent {
    public readonly year = input.required<number>();

    public readonly absences = input.required<LeaveRequestModel[]>();
}
