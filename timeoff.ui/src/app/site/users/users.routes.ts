import { Routes } from "@angular/router";
import { UserListComponent } from "./user-list/user-list.component";
import { ImportComponent } from "./import/import.component";

export const usersRoutes: Routes = [
    {
        path: '',
        component: UserListComponent
    },
    {
        path: 'import',
        component: ImportComponent
    }
]