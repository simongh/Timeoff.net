import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FlashComponent } from '../../../components/flash/flash.component';

@Component({
    selector: 'user-import',
    standalone: true,
    imports: [FlashComponent, RouterLink],
    templateUrl: './user-import.component.html',
    styleUrl: './user-import.component.sass',
})
export class UserImportComponent {}
