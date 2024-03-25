import { Component, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { FlashComponent } from '@components/flash/flash.component';

import { MessagesService } from '@services/messages/messages.service';

import { IntegrationService } from './integration.service';
import { IntegrationModel } from './integration.model';

@Component({
    selector: 'integration',
    standalone: true,
    templateUrl: './integration.component.html',
    styleUrl: './integration.component.scss',
    imports: [FlashComponent],
    providers: [IntegrationService],
})
export class IntegrationComponent {
    public apiEnabled!: boolean;

    public apiKey!: string;

    public name = '';

    public updating = false;

    constructor(
        private readonly apiSvc: IntegrationService,
        private readonly msgsSvc: MessagesService,
        private destroyed: DestroyRef
    ) {
        apiSvc
            .get()
            .pipe(takeUntilDestroyed(destroyed))
            .subscribe({
                next: (data) => this.updateData(data),
                error: (e) => this.error(e),
            });
    }

    public update() {
        this.updating = true;

        this.apiSvc
            .update(this.apiEnabled)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => this.updateData(data),
                error: (e) => this.error(e),
            });
    }

    public regenerate() {
        this.updating = true;

        this.apiSvc
            .regenerate()
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (data) => this.updateData(data),
                error: (e) => this.error(e),
            });
    }

    private updateData(data: IntegrationModel) {
        this.apiEnabled = data.enabled;
        this.apiKey = data.apiKey;

        this.msgsSvc.isSuccess('Settings updated');
        this.updating = false;
    }

    private error(e: any) {
        this.msgsSvc.isError('Unabled to update API settings');
        this.updating = false;
    }
}
