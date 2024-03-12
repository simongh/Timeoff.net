import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { FlashComponent } from "../../../components/flash/flash.component";
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UsersService } from '../../../services/users/users.service';
import { TeamModel } from '../../../models/team.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { switchMap } from 'rxjs';
import { UserModel } from '../../../services/users/user.model';
import { FlashModel, isError } from '../../../components/flash/flash.model';
import { HttpErrorResponse } from '@angular/common/http';
import { YesPipe } from "../../../components/yes.pipe";

@Component({
    selector: 'user-list',
    standalone: true,
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.sass',
    providers: [UsersService],
    imports: [FlashComponent, RouterLink, CommonModule, YesPipe]
})
export class UserListComponent {
  public name: string = '';

  public teams!: TeamModel[]

  public team: number | null = null;

  public users!: UserModel[];

  public messages = new FlashModel();

  constructor(
    private readonly route: ActivatedRoute,
    private destroyed: DestroyRef,
    private usersSvc: UsersService
  ) {
    usersSvc.getTeams()
      .pipe(takeUntilDestroyed(destroyed))
      .subscribe((teams) => {
        this.teams = teams;
      });

    route.queryParamMap
      .pipe(
        takeUntilDestroyed(destroyed),
        switchMap((q) => {
          if (q.has('team')) {
            this.team = Number.parseInt(q.get('team')!);
          } else {
            this.team = null;
          }

          return usersSvc.getUsers(this.team);
        })
      ).subscribe({
        next: (users) => {
          this.users = users;
        },
        error: (e: HttpErrorResponse) => {
          this.messages = isError('Unable to retrieve users');
        }
      });
  }
}
