import { FormControl, FormGroup } from "@angular/forms";

export interface Prices {
  minPrice: number,
  maxPrice: number,
}

export interface Checkboxes {
  veryUsedCheckbox: boolean,
  usedCheckbox: boolean,
  newCheckbox: boolean,
}

export type SortBy = 'PriceAscending' | 'PriceDescending' | 'CreatedAscending' | 'CreatedDescending';

export interface MyForm extends FormGroup {
  controls: {
    veryUsedCheckbox: FormControl<boolean>;
    usedCheckbox: FormControl<boolean>;
    newCheckbox: FormControl<boolean>;
    minPrice: FormControl<number>;
    maxPrice: FormControl<number>;
    sortBy: FormControl<SortBy>;
  };
}

export const INIT_OFFERS_FORM_CONST: MyForm = new FormGroup({
  veryUsedCheckbox: new FormControl(false),
  usedCheckbox: new FormControl(false),
  newCheckbox: new FormControl(false),
  minPrice: new FormControl(null as number),
  maxPrice: new FormControl(null as number),
  sortBy: new FormControl('CreatedAscending' as SortBy),
});