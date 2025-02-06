import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Subject, debounceTime, takeUntil, tap } from 'rxjs';
import { isEqual } from 'lodash';
import { Checkboxes } from '@shared/components/offers-view/offers-filters/interfaces/offers-view.interface';

@Component({
  selector: 'pp-quality-checkboxes',
  standalone: true,
  imports: [CommonModule, MatCheckboxModule, ReactiveFormsModule],
  templateUrl: './quality-checkboxes.component.html',
  styleUrls: ['./quality-checkboxes.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
})
export class QualityCheckboxesComponent implements OnInit, OnDestroy {

  @Output() checkboxesEmitter = new EventEmitter<Checkboxes>();

  private onDestroy$ = new Subject<void>();

  checkboxes: Checkboxes = {
    veryUsedCheckbox: false,
    usedCheckbox: false,
    newCheckbox: false,
  };

  form = new FormGroup({
    veryUsedCheckbox: new FormControl(false),
    usedCheckbox: new FormControl(false),
    newCheckbox: new FormControl(false),
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
    const formCheckboxes = form.value as Checkboxes;

    if (!isEqual(form.value, this.checkboxes)) {
      this.checkboxes = formCheckboxes;
      this.checkboxesEmitter.emit(formCheckboxes);
    }
  }
}
