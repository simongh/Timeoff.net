import { NgClass, NgIf } from "@angular/common";
import { Component, Input } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
    standalone: true,
    templateUrl: 'pager.component.html',
    selector: 'pager',
    imports: [NgIf, RouterLink, NgClass]
})
export class PagerComponent {
    @Input()
    public totalPages! : number;

    @Input()
    public currentPage!: number;

    public get nextPage() {
        return this.currentPage + 1;
    }

    public get previousPage() {
        return this.currentPage -1;
    }
}