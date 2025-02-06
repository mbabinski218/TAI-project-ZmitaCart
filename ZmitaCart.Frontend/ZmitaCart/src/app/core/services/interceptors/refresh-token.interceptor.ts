import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { UserService } from '@core/services/authorization/user.service';
import { ErrorStatus } from '@core/enums/error-status.enum';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';

@Injectable()
export class RefreshTokenInterceptor implements HttpInterceptor {

  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

  constructor(
    private userService: UserService,
  ) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    if (this.userService.isTokenExpired())
      this.userService.logout();

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error instanceof HttpErrorResponse && error.status === ErrorStatus.UNAUTHORIZED) {
          return this.handle401Error(request, next);
        } else {
          return throwError(error);
        }
      })
    );
  }

  private handle401Error(request: HttpRequest<unknown>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next('');

      return this.userService.refreshToken().pipe(
        switchMap((result) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(result.accessToken);
          return next.handle(this.addToken(request, result.accessToken));
        }
        ));
    }

    return this.refreshTokenSubject.pipe(
      filter(token => token != ''),
      take(1),
      switchMap((token) => {
        return next.handle(this.addToken(request, token));
      }
      ));
  }

  private addToken(request: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
    return request.clone({
      setHeaders: {
        'Authorization': `bearer ${token}`,
      }
    });
  }
}
