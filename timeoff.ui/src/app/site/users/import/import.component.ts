import { Component } from '@angular/core';
import { FlashComponent } from '../../../components/flash/flash.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'user-import',
  standalone: true,
  imports: [FlashComponent, RouterLink],
  templateUrl: './import.component.html',
  styleUrl: './import.component.sass'
})
export class ImportComponent {

}
