import { Component, DestroyRef, OnInit, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { FlashComponent } from '@components/flash/flash.component';

import { MessagesService } from '@services/messages/messages.service';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

import { IntegrationService } from './integration.service';
import { IntegrationModel } from './integration.model';

@Component({
    selector: 'integration',
    templateUrl: './integration.component.html',
    styleUrl: './integration.component.scss',
    imports: [FlashComponent],
    providers: [IntegrationService]
})
export class IntegrationComponent implements OnInit {
    protected readonly apiEnabled = signal(true);

    protected readonly apiKey = signal('');

    protected readonly name = inject(LoggedInUserService).companyName;

    protected readonly updating = signal(false);

    constructor(
        private readonly apiSvc: IntegrationService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {}

    public ngOnInit(): void {
        this.apiSvc
            .get()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => this.updateData(data),
            });
    }

    public update() {
        this.updating.set(true);

        this.apiSvc
            .update(this.apiEnabled())
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => this.updateData(data),
            });
    }

    public regenerate() {
        this.updating.set(true);

        this.apiSvc
            .regenerate()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => this.updateData(data),
            });
    }

    private updateData(data: IntegrationModel) {
        this.apiEnabled.set(data.enabled);
        this.apiKey.set(data.apiKey);

        this.msgsSvc.isSuccess('Settings updated');
        this.updating.set(false);
    }
}
