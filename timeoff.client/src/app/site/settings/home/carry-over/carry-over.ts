import { Component, computed, signal, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'ton-carry-over',
  imports: [],
  templateUrl: './carry-over.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './carry-over.scss'
})
export class CarryOver {
    protected readonly currentYear = signal(new Date().getFullYear()).asReadonly();

    protected readonly lastYear = computed(() => this.currentYear() - 1);
}
