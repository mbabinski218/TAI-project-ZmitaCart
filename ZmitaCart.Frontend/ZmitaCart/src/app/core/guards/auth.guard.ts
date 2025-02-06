import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { RoutesPath } from "@core/enums/routes-path.enum";
import { UserService } from "@core/services/authorization/user.service";
import { ToastMessageService } from "@shared/components/toast-message/services/toast-message.service";
import { filter, map, tap } from "rxjs";
import { SharedService } from '@shared/services/shared.service';

export const isUserLoggedInGuard = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const authService = inject(UserService);
  const router = inject(Router);
  if (authService.isAuthenticated())
    return true;

  void router.navigateByUrl(`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`);
};

export const isUserNotLoggedInGuard = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const authService = inject(UserService);
  const router = inject(Router);
  if (!authService.isAuthenticated())
    return true;

  void router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFERS}`);
};

export const userHasAddress = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const authService = inject(UserService);
  const sharedService = inject(SharedService);
  const toastService = inject(ToastMessageService);
  const router = inject(Router);

  if (!authService.isAuthenticated())
    return router.navigateByUrl(`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`);

  sharedService.getUserData().pipe(
    filter((res) => !!res),
    map(({ address }) => address),
    tap((res) => {
      const { apartmentNumber, ...requiredFields } = res;
      const values = Object.values(requiredFields);
      if (values.some((value) => value === null || value === undefined)) {
        toastService.notifyOfError('Przypisz poprawny adres do konta');
        return goToAcc('update');
      }
    })
  ).subscribe();

  const goToAcc = (fragment: string): void => {
    void router.navigate([`${RoutesPath.HOME}/${RoutesPath.ACCOUNT}`], {
      fragment,
    });
  };
};