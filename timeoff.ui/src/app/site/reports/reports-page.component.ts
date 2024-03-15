import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    standalone: true,
    template: '<router-outlet/>',
    selector: 'reports-page',
    imports: [RouterOutlet],
})
export class ReportsPageComponent {}
