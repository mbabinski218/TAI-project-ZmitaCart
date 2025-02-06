import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountOffers } from '@components/account/interfaces/account.interface';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { MyForm } from '@shared/components/offers-view/offers-filters/interfaces/offers-view.interface';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Observable, catchError, of } from 'rxjs';

@Injectable()
export class OffersFilteredService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  getOffers(categoryId = '', title = '', pageNumber: number, form: MyForm): Observable<AccountOffers> {  
    const { veryUsedCheckbox, usedCheckbox, newCheckbox, minPrice, maxPrice, sortBy } = form.value;

    let params = new HttpParams()
      .set('title', title)
      .set('categoryId', categoryId)
      .set('minPrice', minPrice as number ?? '')
      .set('maxPrice', maxPrice as number ?? '')
      .set('sortBy', sortBy as string)
      .set('pageNumber', pageNumber)
      .set('pageSize', 20);

    if (newCheckbox)
      params = params.append('conditions', 'Nowy');
    if (usedCheckbox)
      params = params.append('conditions', 'Dobry');
    if (veryUsedCheckbox)
      params = params.append('conditions', 'Używany');

    return this.http.get<AccountOffers>(`${environment.httpBackend}${Api.OFFER}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }
}
