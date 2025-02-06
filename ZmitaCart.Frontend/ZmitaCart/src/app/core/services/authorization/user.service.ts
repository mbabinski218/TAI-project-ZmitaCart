import { Injectable } from '@angular/core';
import { isEmpty } from 'lodash';
import { LocalStorageService } from '@core/services/localStorage/local-storage.service';
import { KeyStorage } from '@core/enums/key-storage.enum';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Api } from '@core/enums/api.enum';
import { catchError, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { addSeconds, isAfter } from 'date-fns';
import { TokenData, UserAuthorization } from '@components/authentication/interfaces/authentication-interface';
import jwt_decode from 'jwt-decode';
import { UserCredentials } from '@components/account/interfaces/account.interface';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userToken: string;
  private captchaToken: string;

  constructor(
    private localStorageService: LocalStorageService,
    private router: Router,
    private http: HttpClient,
  ) { }

  setUserToken(token: string): void {
    this.userToken = token;

    this.localStorageService.setItem<string>(KeyStorage.USER_TOKEN, token);
  }

  getUserToken(): string {
    return this.userToken;
  }

  setCaptchaToken(token: string): void {
    this.captchaToken = token;
  }

  getCaptchaToken(): string {
    return this.captchaToken;
  }

  isAuthenticated(): boolean {
    return !isEmpty(this.user());
  }

  isUserAdministrator(): boolean {
    const user = this.user();
    return user.role === 'Administrator';
  }

  userAuthorization(): TokenData {
    return this.user();
  }

  logout(): void {
    this.clearAll();
  }

  isTokenExpired(): boolean {
    const { expires_at } = this.user();

    return isAfter(new Date(), new Date(expires_at));
  }

  refreshToken(): Observable<UserAuthorization> {

    if (isEmpty(this.localStorageService.getItem<TokenData>(KeyStorage.USER)))
      this.clearAll();

    const userData = {
      grantType: 'refresh_token',
      refreshToken: this.localStorageService.getItem<TokenData>(KeyStorage.USER).refreshToken,
    };

    return this.http.post<UserAuthorization>(`${environment.httpBackend}${Api.LOGIN}`, userData)
      .pipe(
        tap((token) => {
          const myToken = token;
          this.setUserStorage(myToken);
          this.setUserToken(myToken.accessToken);
        }),
      );
  }

  private user(): TokenData {
    return this.localStorageService.getItem<TokenData>(KeyStorage.USER);
  }

  private clearAll(): void {
    this.localStorageService.clear();
    void this.router.navigate(['/']);
  }

  private setUserStorage(token: UserAuthorization) {
    const decodedToken: TokenData = jwt_decode(token.accessToken);

    const { email, exp, firstName, id, iss, lastName, role } = decodedToken;
    const refreshToken = token.refreshToken;

    this.localStorageService.setItem<TokenData>(KeyStorage.USER, {
      refreshToken, email, firstName, id, iss, lastName, role, expires_at: addSeconds(new Date(), exp).toISOString()
    });
  }
}
