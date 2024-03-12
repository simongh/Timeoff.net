import { Component, DestroyRef, Input } from '@angular/core';
import { FlashComponent } from '../../../components/flash/flash.component';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { ValidatorMessageComponent } from '../../../components/validator-message/validator-message.component';
import { UserModel } from '../../../models/user.model';
import { CommonModule } from '@angular/common';
import { TeamsService } from '../../../services/teams/teams.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { FlashModel, hasErrors, isError, isSuccess } from '../../../components/flash/flash.model';
import { combineLatest, switchMap } from 'rxjs';

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
    return this.editForm.controls.name.value;
  }

  public get editForm() {
    return this.teamSvc.form;
  }

  public users: UserModel[] =[];

  public allowances = new Array<number>;

  public messages = new FlashModel();

  public id!: number;

  constructor(
    private destroyed: DestroyRef,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly teamSvc: TeamsService
  ) {
    for(let i = 0; i <= 50; i += 0.5) {
      this.allowances.push(i);
    }
    
    combineLatest([
      this.teamSvc.getUsers()
        .pipe(takeUntilDestroyed(this.destroyed)),
      route.paramMap
        .pipe(
          takeUntilDestroyed(destroyed),
          switchMap((p) => {
            if (p.has("id")) {
              this.id = Number.parseInt(p.get("id")!);
            } else {
              throw new Error("No team id specified");
            }

            return this.teamSvc.get(this.id);
          })
        )
    ]).subscribe(([users, data]) => {
      this.users = users;

      this.editForm.setValue({
        ...data,
        managerId: data.manager.id,
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
        next: () => this.messages = isSuccess(`Team ${this.name} was updated`),
        error: (e: HttpErrorResponse) => {
          if (e.status == 400) {
            this.messages = hasErrors(e.error.errors);
          } else {
            this.messages = isError(`Unable to update team ${this.name}`);
          }
        }
      });
  }

  public delete() {
    this.teamSvc.delete(this.id)
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe({
        next: () => {
          this.router.navigate(['settings','teams'], {
            state: {
              messages: `Team ${this.name} was deleted`
            }
          });
        },
        error: (e: HttpErrorResponse) => {
          if (e.status == 400) {
            this.messages = hasErrors(e.error.errors);
          } else {
            this.messages = isError('Unable to delete the team');
          }
        }
      });
  };
}
