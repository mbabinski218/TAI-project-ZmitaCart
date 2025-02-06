import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Observable, catchError, of, map } from 'rxjs';

@Injectable()
export class OfferSingleService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  deleteOffer(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.httpBackend}${Api.OFFER_SINGLE}`.replace(':id', id)).pipe(
      map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(false);
      })
    );
  }

  buy(offerId: number, quantity: number): Observable<boolean> {
    return this.http.post<boolean>(`${environment.httpBackend}${Api.OFFER_BUY}`, { offerId, quantity }).pipe(
      map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(false);
      })
    );
  }
}
