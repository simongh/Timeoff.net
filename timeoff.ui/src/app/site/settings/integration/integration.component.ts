import { Component, DestroyRef } from '@angular/core';
import { FlashComponent } from "../../../components/flash/flash.component";
import { IntegrationService } from './integration.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IntegrationModel } from './integration.model';
import { FlashModel, isError, isSuccess } from '../../../components/flash/flash.model';

@Component({
    selector: 'integration',
    standalone: true,
    templateUrl: './integration.component.html',
    styleUrl: './integration.component.sass',
    imports: [FlashComponent],
    providers: [IntegrationService],
})
export class IntegrationComponent {
  public apiEnabled!: boolean;

  public apiKey!: string;

  public name = '';

  public message = new FlashModel();

  public updating = false;

  constructor (
    private readonly apiSvc: IntegrationService,
    private destroyed: DestroyRef) {
      apiSvc.get()
        .pipe(takeUntilDestroyed(destroyed))
        .subscribe({
          next: (data)=>this.updateData(data),
          error: (e) => this.error(e)
        })
    }

  public update() {
    this.updating = true;

    this.apiSvc.update(this.apiEnabled)
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe({
        next: (data) => this.updateData(data),
        error: (e) => this.error(e)
      });
  }

  public regenerate() {
    this.updating = true;
    
    this.apiSvc.regenerate()
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe({
        next: (data) => this.updateData(data),
        error: (e) => this.error(e)
      });
  }

  private updateData(data: IntegrationModel)
  {
    this.apiEnabled = data.enabled;
    this.apiKey = data.apiKey;

    this.message = isSuccess('Settings updated');
    this.updating = false;
  }

  private error(e: any) {
    this.message = isError('Unabled to update API settings');
    this.updating = false;
  }
}
