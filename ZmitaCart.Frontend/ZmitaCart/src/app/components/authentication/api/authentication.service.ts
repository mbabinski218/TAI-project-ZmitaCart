import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenData, UserAuthorization, UserLogin, UserRegister } from '@components/authentication/interfaces/authentication-interface';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { Api } from '@core/enums/api.enum';
import { environment } from '@env/environment';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { UserService } from '@core/services/authorization/user.service';
import { LocalStorageService } from '@core/services/localStorage/local-storage.service';
import jwt_decode from 'jwt-decode';
import { KeyStorage } from '@core/enums/key-storage.enum';
import { addSeconds } from 'date-fns';

@Injectable()
export class AuthenticationService {

  constructor(
    private http: HttpClient,
    private toastMessageService: ToastMessageService,
    private userService: UserService,
    private localStorageService: LocalStorageService,
  ) { }

  register(userData: Partial<UserRegister>): Observable<boolean> {
    return this.http.post<unknown>(`${environment.httpBackend}${Api.REGISTER}`, userData).pipe(
      map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(false);
      })
    );
  }

  login(userData: Partial<UserLogin>): Observable<UserAuthorization> {
    return this.http.post<UserAuthorization>(`${environment.httpBackend}${Api.LOGIN}`, { ...userData, grantType: "password" }).pipe(
      tap((token) => {
        this.setUserStorage(token);
        this.userService.setUserToken(token.accessToken);
      }),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(null);
      })
    );
  }

  googleLogin(token: string): Observable<UserAuthorization> {
    return this.http.post<UserAuthorization>(`${environment.httpBackend}${Api.LOGIN}`, { idToken: token, grantType: "google" }).pipe(
      tap((token) => {
        this.setUserStorage(token);
        this.userService.setUserToken(token.accessToken);
      }),
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(null);
      })
    );
  }

  private setUserStorage(token: UserAuthorization) {
    const decodedToken: TokenData = jwt_decode(token.accessToken);

    const { email, exp, firstName, id, iss, lastName, role } = decodedToken;
    const refreshToken = token.refreshToken;

    this.localStorageService.setItem<TokenData>(KeyStorage.USER, {
      refreshToken, email, firstName, id, iss, lastName, role, expires_at: addSeconds(new Date(), exp).toISOString()
    });
  }

  resendEmail(email: string): Observable<boolean> {
    return this.http.post<boolean>(`${environment.httpBackend}${Api.RESEND_EMAIL}?email=${email}`, {}).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(false);
      })
    );
  }

  confirmEmail(id: string, token: string): Observable<boolean> {
    console.log(id, token);

    return this.http.post<boolean>(`${environment.httpBackend}${Api.CONFIRM_EMAIL}?Id=${id}&Token=${token}`, {}).pipe(
      catchError((err: HttpErrorResponse) => {
        const error = err.error as string[];
        this.toastMessageService.notifyOfError(error[0]);
        return of(false);
      })
    );
  }
}
