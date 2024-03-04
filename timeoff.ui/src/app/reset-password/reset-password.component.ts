import { Component, DestroyRef, Input, OnInit } from '@angular/core';
import { FlashComponent } from "../components/flash/flash.component";
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { compareValidator } from '../components/validators';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ValidatorMessageComponent } from '../components/validator-message/validator-message.component';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    standalone: true,
    templateUrl: './reset-password.component.html',
    styleUrl: './reset-password.component.sass',
    imports: [FlashComponent,CommonModule,ReactiveFormsModule,ValidatorMessageComponent]
})
export class ResetPasswordComponent implements OnInit {
  public passwordForm = this.fb.group({
    current:[''],
    password:['',[Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', []]
  },
  {
    validators: [compareValidator('password','confirmPassword')]
  });

  @Input()
  public set t(value: string) {
    if (!!value) {
      this.token = value;
    }
  }

  public showCurrent!: boolean;

  public messages: string[] = [];

  public errors: string[] = [];

  public submitting = false;

  private token: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authSvc: AuthService,
    private destroyed: DestroyRef) {}

  public ngOnInit(): void {
    this.showCurrent = this.authSvc.isUserLoggedIn;

    if (this.showCurrent){
      this.passwordForm.controls.current.addValidators(Validators.required);
    } else{
      if (!this.token){
        this.errors = ['Invalid reset link'];
        this.submitting = true;
      }
    }
  }

  public save() {
    this.passwordForm.markAllAsTouched();

    if (this.passwordForm.invalid){
      return;
    }

    this.submitting = true;

    const f = this.passwordForm.value;
    this.authSvc.resetPassword({
      password: f.current ? f.current : null,
      newPassword: f.password!,
      confirmPassword: f.confirmPassword!,
      token: this.token
    })
      .pipe(
        takeUntilDestroyed(this.destroyed),
        )
      .subscribe({
        next: (r) => {
          this.messages = ['Password updated successfully'];
          this.submitting = false;
          this.passwordForm.reset();
        },
        error: (e:HttpErrorResponse) => {
          if (e.status === 400) {
            this.errors = e.error.errors;
          }
          else
            this.errors = ['Unable to reset password. Please try again later'];
            this.submitting = false;
        }
      });
  }
}
