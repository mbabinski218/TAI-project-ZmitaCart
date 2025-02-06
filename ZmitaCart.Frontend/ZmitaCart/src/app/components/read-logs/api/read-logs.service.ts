import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Observable, catchError, of } from 'rxjs';
import { Logs } from '../interfaces/read-logs.interface';
import { Nullable } from '@core/types/nullable';

@Injectable()
export class ReadLogsService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
  ) { }

  getLogs(searchText: Nullable<string>, isSuccess: Nullable<boolean>, from: Nullable<string>, to: Nullable<string>, pageNumber: number): Observable<Logs> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', 30);

    if (searchText !== null) {
      params = params.set('searchText', searchText);
    }

    if (isSuccess !== null) {
      params = params.set('isSuccess', isSuccess);
    }

    if (from !== null) {
      console.log(from);
      params = params.set('from', from);
    }

    if (to !== null) {
      params = params.set('to', to);
    }

    return this.http.get<Logs>(`${environment.httpBackend}${Api.LOGS}`, { params }).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of();
      })
    );
  }
}
