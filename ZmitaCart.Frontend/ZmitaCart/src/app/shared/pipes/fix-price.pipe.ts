import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ppFixPrice',
  standalone: true,
})
export class ppFixPricePipe implements PipeTransform {

  transform(value: number): string {
    return value.toLocaleString('pl-PL', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
      useGrouping: true,
    });
  }
}
