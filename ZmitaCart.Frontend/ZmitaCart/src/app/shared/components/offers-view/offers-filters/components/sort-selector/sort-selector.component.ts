import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { Subject, debounceTime, takeUntil, tap } from 'rxjs';
import { isEqual } from 'lodash';
import { SortBy } from '@shared/components/offers-view/offers-filters/interfaces/offers-view.interface';

@Component({
  selector: 'pp-sort-selector',
  standalone: true,
  imports: [CommonModule, MatIconModule, ReactiveFormsModule],
  templateUrl: './sort-selector.component.html',
  styleUrls: ['./sort-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SortSelectorComponent implements OnInit, OnDestroy {

  @Output() sortByEmitter = new EventEmitter<SortBy>();

  private onDestroy$ = new Subject<void>();

  sortBy: SortBy = 'CreatedAscending';

  form = new FormGroup({
    sortBy: new FormControl('CreatedAscending' as SortBy),
  });

  ngOnInit(): void {
    this.form.valueChanges.pipe(
      debounceTime(300),
      tap(() => this.emitValue(this.form)),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  private emitValue(form: FormGroup): void {
    const sortBy = form.value as SortBy;
    
    if (!isEqual(sortBy, this.sortBy)) {
      this.sortBy = sortBy;
      this.sortByEmitter.emit(sortBy);
    }
  }

}
