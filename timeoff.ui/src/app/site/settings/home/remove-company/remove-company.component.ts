import { Component, DestroyRef, input } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

import { MessagesService } from '@services/messages/messages.service';
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
        private readonly msgsSvc: MessagesService,
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
                error: (e: HttpErrorResponse) => {
                    if (e.status == 400) {
                        this.msgsSvc.hasErrors(e.error.errors);
                    } else {
                        this.msgsSvc.isError('Unable to delete company');
                    }
                },
            });
    }
}
