import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from '@core/services/authorization/user.service';
import { LocalStorageService } from '@core/services/localStorage/local-storage.service';
import { KeyStorage } from '@core/enums/key-storage.enum';
import { isEmpty } from 'lodash';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private userService: UserService,
    private localStorageService: LocalStorageService,
  ) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    // if (!this.userService.isAuthenticated() && this.userService.getCaptchaToken()) {
    //   request = request.clone({
    //     setHeaders: {
    //       'Content-Type': 'application/x-www-form-urlencoded',
    //       'g-recaptcha-response': this.userService.getCaptchaToken(),
    //     }
    //   });
    // }
    const token = this.localStorageService.getItem<string>(KeyStorage.USER_TOKEN);

    request = request.clone({
      setHeaders: {
        'Authorization': `bearer ${isEmpty(token) ? undefined : token}`,
        // 'Authorization': `bearer ${this.userService.getUserToken()}`,
      }
    });

    return next.handle(request);
  }
}
