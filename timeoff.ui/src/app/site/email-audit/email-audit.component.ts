import { Component, DestroyRef, OnInit } from '@angular/core';
import { EmailAuditService } from './email-audit.service';
import { ReactiveFormsModule } from '@angular/forms';
import { UserModel } from '../../models/user.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { EmailModel } from './email.model';
import { PagerComponent } from './pager.component';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { FlashComponent } from '../../components/flash/flash.component';
import { FlashModel, isError } from '../../components/flash/flash.model';

@Component({
  selector: 'email-audit',
  standalone: true,
  providers: [EmailAuditService],
  templateUrl: './email-audit.component.html',
  styleUrl: './email-audit.component.sass',
  imports: [ReactiveFormsModule, CommonModule, PagerComponent, FlashComponent],
})
export class EmailAuditComponent implements OnInit {
  public get form() {
    return this.searchSvc.searchForm;
  }

  public messages = new FlashModel();

  public dateFormat = 'yyyy-mm-dd';

  public users: UserModel[] = [];

  public emails: EmailModel[] = [];

  public searching = false;

  public get currentPage() {
    return this.searchSvc.currentPage;
  }

  public get totalPages() {
    return this.searchSvc.totalPages;
  }

  constructor(
    private readonly searchSvc: EmailAuditService,
    private readonly destroyed: DestroyRef,
    private readonly route: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.searchSvc
      .getUsers()
      .pipe(takeUntilDestroyed(this.destroyed))
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
    this.form.reset({
      userId:'',
      start:'',
      end:''
    });
    this.search();
  }

  public searchByUser(userId: number) {
    this.form.controls.userId.setValue(userId.toString());

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

          if (p.has('user')) {
            this.form.controls.userId.setValue(p.get('user')!);
          }

          return this.searchSvc.search();
        })
      )
      .subscribe({
        next: (data) => {
          this.emails = data;
          this.searching = false;
        },
        error: ()=> {
          this.messages = isError('Unable to retrieve emails');
          this.emails = [];
        }
      });
  }
}
