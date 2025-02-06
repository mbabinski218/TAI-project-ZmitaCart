import { ChangeDetectionStrategy, Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SortSelectorComponent } from '@shared/components/offers-view/offers-filters/components/sort-selector/sort-selector.component';
import { QualityCheckboxesComponent } from '@shared/components/offers-view/offers-filters/components/quality-checkboxes/quality-checkboxes.component';
import { PriceInputComponent } from '@shared/components/offers-view/offers-filters/components/price-input/price-input.component';
import { Checkboxes, INIT_OFFERS_FORM_CONST, Prices, SortBy } from '@shared/components/offers-view/offers-filters/interfaces/offers-view.interface';

@Component({
  selector: 'pp-offers-filters',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatCheckboxModule, SortSelectorComponent, QualityCheckboxesComponent, PriceInputComponent],
  templateUrl: './offers-filters.component.html',
  styleUrls: ['./offers-filters.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OffersFiltersComponent {

  @Output() formEmitter = new EventEmitter();

  form = INIT_OFFERS_FORM_CONST;
  hasErrors = false;

  setAll(data: Checkboxes | Prices | SortBy) {
    if (typeof data === 'string') {
      this.form.patchValue({ sortBy: data });
    } else {
      this.form.patchValue(data);
    }
  }

  submit(): void {
    this.formEmitter.emit(this.form.value);
  }
}
