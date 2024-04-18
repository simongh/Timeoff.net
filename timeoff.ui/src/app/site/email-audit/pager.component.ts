import { NgClass, NgIf } from '@angular/common';
import { Component, Input, computed, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    standalone: true,
    templateUrl: 'pager.component.html',
    selector: 'pager',
    imports: [NgIf, RouterLink, NgClass],
})
export class PagerComponent {
    public totalPages = input.required<number>();

    public currentPage = input.required<number>();

    protected readonly nextPage = computed(() => this.currentPage() + 1);

    protected readonly previousPage = computed(() => this.currentPage() - 1);
}
