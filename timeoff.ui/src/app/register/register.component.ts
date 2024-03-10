import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { FlashComponent } from "../components/flash/flash.component";
import { FormBuilder,ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterService } from './register.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop'
import { compareValidator, listValidator } from '../components/validators';
import { RegisterModel } from './register.model';
import { ValidatorMessageComponent } from '../components/validator-message/validator-message.component';
import { HttpErrorResponse } from '@angular/common/http';
import { FlashModel, hasErrors, isSuccess } from '../components/flash/flash.model';

@Component({
    standalone: true,
    templateUrl: 'register.component.html',
    providers: [RegisterService],
    imports: [FlashComponent, ReactiveFormsModule, CommonModule, ValidatorMessageComponent]
})

export class RegisterComponent implements OnInit {
  public registerForm = this.fb.group({
    companyName: ['', [Validators.required]],
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
    country: ['GB'],
    timezone: ['']
  },
  {
    validators:[compareValidator('password','confirmPassword')]
  }
  )

  public countries!: {
    code: string;
    name: string;
  }[];

  public timezones!: {
    name: string;
    description: string;
  }[];

  public messages = new FlashModel();

  public submitting = false;

  private destroyed$ = inject(DestroyRef);

  constructor(
    private fb: FormBuilder,
    private registerSvc: RegisterService)
  {
    ({countries: this.countries, timezones: this.timezones} = this.registerSvc.getOptions());
  }

  ngOnInit(): void {
    this.registerForm.controls.country.addValidators(listValidator(this.countries.map((c)=>c.code)));
    this.registerForm.controls.timezone.addValidators(listValidator(this.timezones.map((t)=>t.name)));

    this.registerForm.controls.timezone.setValue(this.timezones[0].name);
  }

  public register() {
    this.registerForm.markAllAsTouched();
    if (this.registerForm.invalid)
      return;

    this.submitting = true;
    this.registerSvc.register(this.registerForm.value as RegisterModel)
      .pipe(takeUntilDestroyed(this.destroyed$))
      .subscribe({
        next: ()=>{
          this.messages = isSuccess('Company registered successfully. Please login using the details you supplied');
          this.registerForm.reset();
        },
        error: (e: HttpErrorResponse) => {
          this.messages = hasErrors(e.error.errors);
        }
      });
  }
}
