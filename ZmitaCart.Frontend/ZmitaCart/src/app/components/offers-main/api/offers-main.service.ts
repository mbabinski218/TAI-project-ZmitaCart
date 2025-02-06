import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Observable, catchError, of } from 'rxjs';
import { Offers } from '@components/offers-main/interfaces/offers-main.interface';

@Injectable()
export class OfferMainService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  getOffers(categoriesNames: string[]): Observable<Offers[]> {
    let params = new HttpParams()
      .set('size', 12);

    categoriesNames.forEach((name) => {
      params = params.append('categoriesNames', name);
    });

    return this.http.get<Offers[]>(`${environment.httpBackend}${Api.OFFER_MAIN}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of([]);
      })
    );
  }

  getMostPopularCategories(): Observable<string[]> {
    const params = new HttpParams()
      .set('numberOfCategories', 4);

    return this.http.get<string[]>(`${environment.httpBackend}${Api.CATEGORIES_POPULAR}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of([]);
      })
    );
  }
}
