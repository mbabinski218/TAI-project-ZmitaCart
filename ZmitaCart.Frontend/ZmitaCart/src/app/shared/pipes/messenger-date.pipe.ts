import { Pipe, PipeTransform } from '@angular/core';
import { differenceInDays, format } from 'date-fns';
import { pl } from 'date-fns/locale';

@Pipe({
  name: 'ppMessengerDate',
  standalone: true,
})
export class ppMessengerDatePipe implements PipeTransform {

  transform(date: Date): string {
    const dzisiaj = new Date();

    const differenceDays = differenceInDays(dzisiaj, date);

    if (differenceDays === 0) {
      return `Dzisiaj, ${format(date, 'HH:mm')}`;
    } else if (differenceDays === 1) {
      return `Wczoraj, ${format(date, 'HH:mm')}`;
    }
    else if (differenceDays < 8) {
      return format(date, 'dd MMMM', { locale: pl });
    } else if (differenceDays < 31) {
      return "Ponad tydzień temu";
    } else {
      return "Ponad miesiąc temu";
    }
  }
}
