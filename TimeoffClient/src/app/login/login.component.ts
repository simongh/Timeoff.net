import { Component, DestroyRef, inject } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";
import { CommonModule } from "@angular/common";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { FlashComponent } from "../components/flash/flash.component";
import { LoginModel } from "../services/auth/login.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

@Component({
    standalone: true,
    templateUrl: 'login.component.html',
    imports: [RouterLink, CommonModule, ReactiveFormsModule, FlashComponent]
})
export class LoginComponent{
    public allowRegistrations: boolean = true;

    public loginForm = this.fb.group({
        email:['', [Validators.required, Validators.email]],
        password:['', Validators.required]
    });

    public get hasErrors() {
        return this.loginForm.invalid && (this.loginForm.touched)
    }

    private destroyed$ = inject(DestroyRef);

    constructor(
        private authService: AuthService,
        private router: Router,
        private fb: FormBuilder)
    {}

    public login(){
        this.loginForm.markAllAsTouched();
        
        if (!this.loginForm.valid)
            return;

        this.authService.login(this.loginForm.value as LoginModel)
            .pipe(takeUntilDestroyed(this.destroyed$))
            .subscribe();

        this.router.navigate(['']);
    }
    

}