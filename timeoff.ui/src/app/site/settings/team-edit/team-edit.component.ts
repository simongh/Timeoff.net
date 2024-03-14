import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { FlashComponent } from '../../../components/flash/flash.component';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { ValidatorMessageComponent } from '../../../components/validator-message/validator-message.component';
import { UserModel } from '../../../models/user.model';
import { CommonModule } from '@angular/common';
import { TeamsService } from '../../../services/teams/teams.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import {
  FlashModel,
  hasErrors,
  isError,
  isSuccess,
} from '../../../components/flash/flash.model';
import { combineLatest, switchMap } from 'rxjs';
import { MessagesService } from '../../../services/messages/messages.service';

@Component({
  selector: 'app-team-edit',
  standalone: true,
  imports: [
    FlashComponent,
    RouterLink,
    ReactiveFormsModule,
    ValidatorMessageComponent,
    CommonModule,
  ],
  providers: [TeamsService],
  templateUrl: './team-edit.component.html',
  styleUrl: './team-edit.component.sass',
})
export class TeamEditComponent implements OnInit {
  public get name() {
    return this.editForm.controls.name.value;
  }

  public get editForm() {
    return this.teamSvc.form;
  }

  public users: UserModel[] = [];

  public allowances = new Array<number>();

  public messages = new FlashModel();

  public id!: number;

  constructor(
    private readonly route: ActivatedRoute,
    private destroyed: DestroyRef,
    private readonly router: Router,
    private readonly teamSvc: TeamsService,
    private readonly messagesSvc: MessagesService
  ) {
    for (let i = 0; i <= 50; i += 0.5) {
      this.allowances.push(i);
    }
  }

  public ngOnInit(): void {
    combineLatest([
      this.teamSvc.getUsers().pipe(takeUntilDestroyed(this.destroyed)),
      this.route.paramMap.pipe(
        takeUntilDestroyed(this.destroyed),
        switchMap((p) => {
          if (p.has('id')) {
            this.id = Number.parseInt(p.get('id')!);
          } else {
            throw new Error('No team id specified');
          }

          return this.teamSvc.get(this.id);
        })
      ),
    ]).subscribe(([users, data]) => {
      this.users = users;

      this.editForm.setValue({
        name: data.name,
        allowance: data.allowance,
        includePublicHolidays: data.includePublicHolidays,
        isAccruedAllowance: data.isAccruedAllowance,
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
    this.teamSvc
      .update(this.id)
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe({
        next: () =>
          (this.messages = isSuccess(`Team ${this.name} was updated`)),
        error: (e: HttpErrorResponse) => {
          if (e.status == 400) {
            this.messages = hasErrors(e.error.errors);
          } else {
            this.messages = isError(`Unable to update team ${this.name}`);
          }
        },
      });
  }

  public delete() {
    this.teamSvc
      .delete(this.id)
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe({
        next: () => {
          this.messagesSvc.isSuccess(`Team ${this.name} was deleted`);
          this.router.navigate(['settings', 'teams']);
        },
        error: (e: HttpErrorResponse) => {
          if (e.status == 400) {
            this.messages = hasErrors(e.error.errors);
          } else {
            this.messages = isError('Unable to delete the team');
          }
        },
      });
  }
}
