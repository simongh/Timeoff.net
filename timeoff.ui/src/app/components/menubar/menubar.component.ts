import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
    selector: 'menu-bar',
    templateUrl: './menubar.component.html',
    standalone: true,
    imports: [RouterLink]
})
export class MenuBarComponent{}