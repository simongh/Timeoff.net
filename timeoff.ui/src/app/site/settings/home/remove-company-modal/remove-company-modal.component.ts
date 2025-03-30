import { Component, output, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

import { ValidatorMessageComponent } from '@components/validator-message/validator-message.component';

@Component({
    selector: 'remove-company-modal',
    templateUrl: './remove-company-modal.component.html',
    styleUrl: './remove-company-modal.component.scss',
    imports: [ValidatorMessageComponent, ReactiveFormsModule]
})
export class RemoveCompanyModalComponent {
    protected readonly companyName = new FormControl<string>('', [Validators.required]);

    protected readonly submitting = signal(false);

    public readonly deleted = output<string>();

    public delete() {
        this.companyName.markAllAsTouched();

        if (this.companyName.invalid) {
            return;
        }

        this.deleted.emit(this.companyName.value!);
    }
}
