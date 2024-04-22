import { signal } from '@angular/core';

export function getAllowances() {
    const allowance = new Array<number>();

    for (let index = 0; index <= 50; index += 0.5) {
        allowance.push(index);
    }

    return signal(allowance).asReadonly();
}
