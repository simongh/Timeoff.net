import { Component, DestroyRef, inject } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { AuthService } from "../services/auth/auth.service";
import { CommonModule } from "@angular/common";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { FlashComponent } from "../components/flash/flash.component";
import { LoginModel } from "../services/auth/login.model";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { ValidatorMessageComponent } from "../components/validator-message/validator-message.component";
import { FlashModel, hasErrors, isError } from "../components/flash/flash.model";

@Component({
    standalone: true,
    templateUrl: 'login.component.html',
    imports: [RouterLink, CommonModule, ReactiveFormsModule, FlashComponent,ValidatorMessageComponent ],
    providers:[]
})
export class LoginComponent{
    public allowRegistrations: boolean = true;

    public submitting = false;
    
    public messages = new FlashModel();

    public loginForm = this.fb.group({
        email:['', [Validators.required, Validators.email]],
        password:['', Validators.required]
    });

    constructor(
        private authService: AuthService,
        private router: Router,
        private fb: FormBuilder,
        private destroyed: DestroyRef)
    {}

    public login(){
        this.loginForm.markAllAsTouched();
        
        if (!this.loginForm.valid)
            return;

        this.submitting = true;
        this.authService.login(this.loginForm.value as LoginModel)
            .pipe(takeUntilDestroyed(this.destroyed))
            .subscribe({
                next: (r) => {
                    if (this.authService.isUserLoggedIn) {
                        this.router.navigate(['']);
                    } else {
                        this.messages = !!r ? hasErrors(r) : isError("Invalid credentials");
                        this.loginForm.controls.password.setValue('');
                        this.loginForm.markAsUntouched();
                    }

                    this.submitting = false;
                },
                error: () => {
                    this.submitting = false;

                    this.messages = isError('Login failed');
                },
            });
    }
}