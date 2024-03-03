import { Component, DestroyRef } from '@angular/core';
import { EmailAuditService } from './email-audit.service';
import { ReactiveFormsModule } from '@angular/forms';
import { UserModel } from '../../models/user.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { EmailModel } from './email.model';
import { PagerComponent } from "./pager.component";
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';

@Component({
    selector: 'email-audit',
    standalone: true,
    providers: [EmailAuditService],
    templateUrl: './email-audit.component.html',
    styleUrl: './email-audit.component.sass',
    imports: [ReactiveFormsModule, CommonModule, PagerComponent]
})
export class EmailAuditComponent {
  public get form() {
    return this.searchSvc.searchForm;
  }

  public dateFormat = 'yyyy-MM-dd';

  public users!: UserModel[]

  public emails!: EmailModel[];

  public searching = false;

  public get currentPage() {
    return this.searchSvc.currentPage;
  }

  public get totalPages() {
    return this.searchSvc.totalPages;
  }
  
  public get showReset() {
    if (this.form.touched) {
       return null;
    } else {
      return 'disabled';
    }
  }

  constructor(
    private readonly searchSvc: EmailAuditService,
    private readonly destroyed: DestroyRef,
    private readonly route: ActivatedRoute,
  ) {
    searchSvc.getUsers()
      .pipe(takeUntilDestroyed(destroyed))
      .subscribe((data) => {
            this.users = data;
      });

      this.find();
  }

  public search() {
    this.searchSvc.currentPage = 1;
    this.find();
  }

  public reset() {
    this.form.reset();
    this.search();
  }

  public searchByUser(userId: number) {
    this.form.controls.userId.setValue(userId);

    this.search();
  }

  private find() {
    this.searching = true;

    this.route.queryParamMap
      .pipe(
        takeUntilDestroyed(this.destroyed),
        switchMap((p) => {
          if (p.has('page')) {
            this.searchSvc.currentPage = Number.parseInt(p.get('page')!);
          } else {
            this.searchSvc.currentPage = 1;
          }

          return this.searchSvc.search();
        })
      ).subscribe((data)=>{
        this.emails = data;
        this.searching = false;
      });
  }
}
