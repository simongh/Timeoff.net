import { Component } from '@angular/core';
import { FlashComponent } from '../../components/flash/flash.component';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'feeds',
  standalone: true,
  imports: [FlashComponent, NgIf,RouterLink],
  templateUrl: './feeds.component.html',
  styleUrl: './feeds.component.sass'
})
export class FeedsComponent {
  public showTeamView = true;
}
