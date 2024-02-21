import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { FlashComponent } from "../components/flash/flash.component";
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { compareValidator } from '../components/validators';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { ResetPasswordModel } from '../services/auth/reset-password.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
    standalone: true,
    templateUrl: './reset-password.component.html',
    styleUrl: './reset-password.component.sass',
    imports: [FlashComponent,CommonModule,ReactiveFormsModule,]
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

  public showCurrent!: boolean;

  public messages: string[] = [];

  public errors: string[] = [];

  public submitting = false;

  private token: string | null = null;

  private destroyed$ = inject(DestroyRef);

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authSvc: AuthService) {}

  ngOnInit(): void {
    this.route.queryParamMap
      .subscribe((p)=>{
        this.showCurrent = this.authSvc.isUserLoggedIn;

          if (this.showCurrent){
            this.passwordForm.controls.current.addValidators(Validators.required);
          } else{
            this.token = p.get('t');

            if (!this.token){
              this.errors = ['Invalid reset link'];
              this.submitting = true;
            }
          }
      })
  }

  public save() {
    this.passwordForm.markAllAsTouched();

    if (this.passwordForm.invalid){
      return;
    }

    this.submitting = true;

    this.authSvc.resetPassword({
      ...this.passwordForm.value as ResetPasswordModel,
      token: this.token
    })
      .pipe(takeUntilDestroyed(this.destroyed$))
      .subscribe(() => {
        this.messages = ['Password updated successfully'];
        this.submitting = false;
      });
  }
}
