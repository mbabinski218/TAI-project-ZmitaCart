import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { CredentialsForm, AccountOffers } from '@components/account/interfaces/account.interface';
import { Observable, catchError, map, of } from 'rxjs';
import { BoughtOffers } from '@components/account/interfaces/account.interface';

@Injectable()
export class AccountService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  sendForm(userData: Partial<CredentialsForm>): Observable<boolean> {
    return this.http.put<unknown>(`${environment.httpBackend}${Api.USER_CREDENTIALS_UPDATE}`, userData).pipe(
      map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(false);
      })
    );
  }

  getUserOffers(pageNumber: number): Observable<AccountOffers> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', 10);

    return this.http.get<AccountOffers>(`${environment.httpBackend}${Api.USER_OFFERS}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }

  // createNewChat(id: string): Observable<number> {
  //   const params = new HttpParams()
  //     .set('offerId', id);

  //   return this.http.post<number>(`${environment.httpBackend}${Api.CONVERSATION}`, {}, { params }).pipe(
  //     catchError((err: HttpErrorResponse) => {
  //       const error = err.error as string[];
  //       this.toastMessageService.notifyOfError(error[0]);
  //       return of();
  //     })
  //   );
  // }

  getFavourites(pageNumber: number): Observable<AccountOffers> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', 10);

    return this.http.get<AccountOffers>(`${environment.httpBackend}${Api.OFFER_FAVOURITES}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }

  getBought(pageNumber: number): Observable<BoughtOffers> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', 10);

    return this.http.get<BoughtOffers>(`${environment.httpBackend}${Api.OFFER_BOUGHT}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }
}
