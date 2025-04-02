import { NgIf } from "@angular/common";
import { Component, computed, input } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
    templateUrl:'navigation.component.html',
    selector: 'navigation',
    imports: [RouterLink, NgIf]
})
export class NavigationComponent
{
    public readonly year = input.required<number>();

    public readonly showFullYear = input.required<boolean>();

    protected readonly nextYear = computed(() => this.year() + 1);

    protected readonly lastYear = computed(() => this.year() - 1);

}