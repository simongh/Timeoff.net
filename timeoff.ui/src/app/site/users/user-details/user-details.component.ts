import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { FlashModel } from '../../../components/flash/flash.model';
import { FlashComponent } from "../../../components/flash/flash.component";

@Component({
    selector: 'user-details',
    standalone: true,
    templateUrl: './user-details.component.html',
    styleUrl: './user-details.component.sass',
    imports: [CommonModule, RouterLink, FlashComponent,RouterLinkActive]
})
export class UserDetailsComponent {
  @Input()
  public name!: string

  @Input()
  public isActive!: boolean

  @Input()
  public id!: number

  @Input()
  public messages!: FlashModel
}
