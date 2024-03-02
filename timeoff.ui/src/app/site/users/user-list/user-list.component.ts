import { Component, DestroyRef, OnInit } from '@angular/core';
import { FlashComponent } from "../../../components/flash/flash.component";
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
    selector: 'user-list',
    standalone: true,
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.sass',
    imports: [FlashComponent, RouterLink]
})
export class UserListComponent implements OnInit {
  public name: string = '';

  public team: number | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private destroyed: DestroyRef) {}

  public ngOnInit(): void {
    this.route.queryParamMap
      .pipe(takeUntilDestroyed(this.destroyed))
      .subscribe((r) => {
        if (r.has('team')) {
            this.team = Number.parseInt(r.get('team')!);
        } else {
            this.team = null;
        }

      });
  }
}
