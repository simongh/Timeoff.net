import { Component, DestroyRef, input } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

import { CompanyService } from '@services/company/company.service';

import { RemoveCompanyModalComponent } from '../remove-company-modal/remove-company-modal.component';

@Component({
    selector: 'remove-company',
    standalone: true,
    templateUrl: './remove-company.component.html',
    styleUrl: './remove-company.component.scss',
    imports: [RemoveCompanyModalComponent],
})
export class RemoveCompanyComponent {
    public readonly companyName = input.required<string>();

    constructor(
        private readonly companySvc: CompanyService,
        private destroyed: DestroyRef,
        private readonly router: Router
    ) {}

    public deleteCompany(name: string) {
        this.companySvc
            .deleteCompany(name)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: () => {
                    this.router.navigateByUrl('/logout');
                },
            });
    }
}
