import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({
    standalone:true,
    selector: 'users-page',
    templateUrl: 'users-page.component.html',
    imports:[RouterOutlet]
})
export class UsersPageComponent {}