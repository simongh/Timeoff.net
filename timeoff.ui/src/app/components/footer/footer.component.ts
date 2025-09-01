import { Component, signal } from '@angular/core';

@Component({
    selector: 'site-footer',
    standalone: true,
    templateUrl: 'footer.component.html',
    styleUrl: 'footer.component.scss',
})
export class FooterComponent {
    protected readonly year = signal(() => new Date().getFullYear()).asReadonly();
}
