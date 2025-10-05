import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    standalone: true,
    name: 'yes',
})
export class YesPipe implements PipeTransform {
    public transform(value: boolean) {
        return value ? 'Yes' : '';
    }
}
