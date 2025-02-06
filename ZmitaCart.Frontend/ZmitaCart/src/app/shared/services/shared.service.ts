import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { UserCredentials } from '@components/account/interfaces/account.interface';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { BehaviorSubject, Observable, Subject, catchError, map, of, takeUntil, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService implements OnDestroy {

  favouritesCount$ = new BehaviorSubject<number>(0);
  private onDestroy$ = new Subject<void>();

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  addToFavourites(id: number, wasFavourite: boolean): Observable<boolean> {
    return this.http.post(`${environment.httpBackend}${Api.OFFER_ADD_TO_FAVOURITES}`.replace(':id', id.toString()), {}).pipe(
      map(() => true),
      tap(() => {
        let current = this.favouritesCount$.value;
        current = wasFavourite ? current - 1 : current + 1;
        this.favouritesCount$.next(current);
      }),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];

        this.toastMessageService.notifyOfError(error[0]);

        return of(false);
      })
    );
  }

  getOffer(id: string): Observable<SingleOffer> {
    return this.http.get<SingleOffer>(`${environment.httpBackend}${Api.OFFER_SINGLE}`.replace(':id', id)).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }

  getUserData(): Observable<UserCredentials> {
    return this.http.get<UserCredentials>(`${environment.httpBackend}${Api.USER_CREDENTIALS}`).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }

  getFavouritesCount(): Observable<number> {
    this.getFavouritesCountHelper().pipe(
      tap((res) => this.favouritesCount$.next(res)),
      takeUntil(this.onDestroy$),
    ).subscribe();

    return this.favouritesCount$.asObservable();
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  private getFavouritesCountHelper(): Observable<number> {
    return this.http.get<number>(`${environment.httpBackend}${Api.OFFER_FAVOURITES_COUNT}`).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }
}
