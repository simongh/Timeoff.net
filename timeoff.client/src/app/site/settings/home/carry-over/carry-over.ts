import { Component, computed, signal } from '@angular/core';

@Component({
  selector: 'ton-carry-over',
  imports: [],
  templateUrl: './carry-over.html',
  styleUrl: './carry-over.scss'
})
export class CarryOver {
    protected readonly currentYear = signal(new Date().getFullYear()).asReadonly();

    protected readonly lastYear = computed(() => this.currentYear() - 1);
}
