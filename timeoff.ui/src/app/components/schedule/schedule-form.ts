import { FormBuilder } from '@angular/forms';

export type ScheduleFormGroup = ReturnType<typeof createScheduleForm>;

export function createScheduleForm(fb: FormBuilder) {
    return fb.group({
        monday: [false],
        tuesday: [false],
        wednesday: [false],
        thursday: [false],
        friday: [false],
        saturday: [false],
        sunday: [false],
    });
}
