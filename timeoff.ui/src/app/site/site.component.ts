import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({
    standalone: true,
    templateUrl: 'site.component.html',
    imports:[RouterOutlet]
})
export class SiteComponent{}