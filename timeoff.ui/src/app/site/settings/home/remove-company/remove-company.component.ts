import { Component, DestroyRef, inject, input } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

import { CompanyService } from '@services/company/company.service';

import { RemoveCompanyModalComponent } from '../remove-company-modal/remove-company-modal.component';

@Component({
    selector: 'remove-company',
    templateUrl: './remove-company.component.html',
    styleUrl: './remove-company.component.scss',
    imports: [RemoveCompanyModalComponent]
})
export class RemoveCompanyComponent {
    readonly #destroyed = inject(DestroyRef);

    readonly #companySvc = inject(CompanyService);

    readonly #router = inject(Router);

    public readonly companyName = input.required<string>();

    public deleteCompany(name: string) {
        this.#companySvc
            .deleteCompany(name)
            .pipe(takeUntilDestroyed(this.#destroyed))
            .subscribe({
                next: () => {
                    this.#router.navigateByUrl('/logout');
                },
            });
    }
}
