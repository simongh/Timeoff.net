import { inject } from "@angular/core"
import { Router } from "@angular/router"
import { AuthService } from "./auth.service";

export const authGuard = () => {
    const router = inject(Router);
    const authService = inject(AuthService);

    if(authService.isUserLoggedIn){
        return true;
    }

    authService.getToken().subscribe();

    if (authService.isUserLoggedIn) {
        return true;
    }
        
    return router.createUrlTree(['login']);
}