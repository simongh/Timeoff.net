import { Component } from '@angular/core';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FlashComponent } from '../../components/flash/flash.component';

@Component({
    selector: 'feeds',
    standalone: true,
    imports: [FlashComponent, NgIf, RouterLink],
    templateUrl: './feeds.component.html',
    styleUrl: './feeds.component.sass',
})
export class FeedsComponent {
    public showTeamView = true;
}
