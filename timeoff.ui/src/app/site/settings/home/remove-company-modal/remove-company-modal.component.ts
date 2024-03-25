import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';

import { ValidatorMessageComponent } from "@components/validator-message/validator-message.component";

@Component({
    selector: 'remove-company-modal',
    standalone: true,
    templateUrl: './remove-company-modal.component.html',
    styleUrl: './remove-company-modal.component.scss',
    imports: [ValidatorMessageComponent, ReactiveFormsModule]
})
export class RemoveCompanyModalComponent {
    public companyName =new FormControl<string>('',[Validators.required])

    public submitting = false;

    @Output()
    public onDelete = new EventEmitter<string>();

    public delete() {
        this.companyName.markAllAsTouched();

        if (this.companyName.invalid) {
            return;
        }

        this.onDelete.emit(this.companyName.value!);
    }
}
