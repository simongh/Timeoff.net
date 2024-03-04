import { Component, DestroyRef, Input } from '@angular/core';
import { FlashComponent } from '../../../components/flash/flash.component';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ValidatorMessageComponent } from '../../../components/validator-message/validator-message.component';
import { UserModel } from '../../../models/user.model';
import { CommonModule } from '@angular/common';
import { TeamsService } from '../../../services/teams/teams.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-team-edit',
  standalone: true,
  imports: [FlashComponent, RouterLink, ReactiveFormsModule,ValidatorMessageComponent, CommonModule],
  providers: [TeamsService],
  templateUrl: './team-edit.component.html',
  styleUrl: './team-edit.component.sass'
})
export class TeamEditComponent {
  public get name() {
    return this.editForm.value.name;
  }

  @Input()
  public id!: number;

  public get editForm() {
    return this.teamSvc.form;
  }

  public users: UserModel[] =[];

  public allowances = new Array<number>;

  public messages: string[] = [];

  public errors: string[] = [];

  constructor(
    private readonly fb: FormBuilder,
    private destroyed: DestroyRef,
    private readonly router: Router,
    private readonly teamSvc: TeamsService
  ) {
    for(let i = 0; i <= 50; i += 0.5) {
      this.allowances.push(i);
    }
    
    this.teamSvc.get(this.id)
      .pipe(takeUntilDestroyed(destroyed))
      .subscribe((data) => {
        this.editForm.setValue({
          name: data.name,
          allowance: data.allowance,
          manager: data.manager.id,
          includePublicHolidays: data.includePublicHolidays,
          accruedAllowance:data.isAccruedAllowance,
        });
      });
  }

  public save() {
    this.editForm.markAllAsTouched();

    if (this.editForm.invalid) {
      return;
    }

    const f = this.editForm.value;
    this.teamSvc.update(this.id)
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe({
        next: () => this.messages = [`Team ${this.name} was updated`],
        error: (e: HttpErrorResponse) => {
          if (e.status == 400) {
            this.errors = e.error.errors;
          } else {
            this.errors = [`Unable to update team ${this.name}`];
          }
        }
      });
  }

  public delete() {
    this.teamSvc.delete(this.id)
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe({
        next: () => {
          this.router.navigateByUrl('/settings/teams', {
            state: {
              messages: `Team ${this.name} was deleted`
            }
          });
        },
        error: (e: HttpErrorResponse) => {
          if (e.status == 400) {
            this.errors = e.error.errors;
          } else {
            this.errors = ['Unable to delete the team'];
          }
        }
      });
  };
}
