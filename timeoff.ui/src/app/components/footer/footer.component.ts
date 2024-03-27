import { Component } from '@angular/core';

@Component({
    selector: 'site-footer',
    standalone: true,
    templateUrl: 'footer.component.html',
    styleUrl: 'footer.component.scss',
})
export class FooterComponent {
    public year = new Date().getFullYear();
}
