import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { FlashComponent } from "../../../components/flash/flash.component";
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
    selector: 'user-list',
    standalone: true,
    templateUrl: './user-list.component.html',
    styleUrl: './user-list.component.sass',
    imports: [FlashComponent, RouterLink]
})
export class UserListComponent {
  public name: string = '';

  @Input()
  public team: number | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private destroyed: DestroyRef) {}
}
