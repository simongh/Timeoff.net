import { Pipe, PipeTransform } from '@angular/core';

import { datePart } from '@models/types';

@Pipe({
    standalone: true,
    name: 'datePart',
})
export class DatePartPipe implements PipeTransform {
    public transform(value: datePart) {
        if (value == datePart.afternoon) {
            return 'afternoon';
        } else if (value == datePart.morning) {
            return 'morning';
        } else {
            return '';
        }
    }
}
