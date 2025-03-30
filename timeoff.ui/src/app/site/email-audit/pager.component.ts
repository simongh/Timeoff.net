import { NgClass, NgIf } from '@angular/common';
import { Component, computed, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
    templateUrl: 'pager.component.html',
    selector: 'pager',
    imports: [NgIf, RouterLink, NgClass]
})
export class PagerComponent {
    public readonly totalPages = input.required<number>();

    public readonly currentPage = input.required<number>();

    protected readonly nextPage = computed(() => this.currentPage() + 1);

    protected readonly previousPage = computed(() => this.currentPage() - 1);
}
