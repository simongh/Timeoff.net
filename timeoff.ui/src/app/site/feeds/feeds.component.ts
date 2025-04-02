import { Component, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

import { FlashComponent } from '@components/flash/flash.component';
import { LoggedInUserService } from '@services/logged-in-user/logged-in-user.service';

@Component({
    selector: 'feeds',
    imports: [FlashComponent, NgIf, RouterLink],
    templateUrl: './feeds.component.html',
    styleUrl: './feeds.component.scss'
})
export class FeedsComponent {
    protected readonly showTeamView = inject(LoggedInUserService).showTeamView;
}
