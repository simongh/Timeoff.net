import { Component } from "@angular/core";
import { MenuBarComponent } from "../components/menubar/menubar.component";
import { RouterOutlet } from "@angular/router";

@Component({
    standalone: true,
    templateUrl: 'site.component.html',
    imports:[MenuBarComponent, RouterOutlet]
})
export class SiteComponent{}