import { Component, DestroyRef, effect, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

import { AuthService } from '@app-types/auth/auth.service';

@Component({
  selector: 'ton-logout',
  imports: [],
  template: '',
})
export class Logout implements OnInit {
  readonly #authSvc = inject(AuthService);

  readonly #destroyed = inject(DestroyRef);

  readonly #router = inject(Router);

  public ngOnInit(): void {
    this.#authSvc
      .logout()
      .pipe(takeUntilDestroyed(this.#destroyed))
      .subscribe(() => {
        this.#router.navigateByUrl('/');
      });
  }
}
