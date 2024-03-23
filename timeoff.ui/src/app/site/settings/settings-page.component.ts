import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    standalone: true,
    selector: 'settings-page',
    templateUrl: 'settings-page.component.html',
    imports: [RouterOutlet],
})
export class SettingsPageComponent {}
