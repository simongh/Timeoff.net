import { Component, input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { TeamSelectComponent } from '@components/team-select/team-select.component';
import { DateInputDirective } from '@components/date-input.directive';

import { SearchFormGroup } from './allowance-usage.service';

@Component({
    templateUrl: 'filter.component.html',
    selector: 'filter',
    imports: [DateInputDirective, TeamSelectComponent, ReactiveFormsModule],
})
export class FilterComponent {
    public form = input.required<SearchFormGroup>();
}
