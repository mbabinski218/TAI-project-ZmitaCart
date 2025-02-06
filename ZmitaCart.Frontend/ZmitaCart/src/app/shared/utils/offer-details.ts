import { Router } from "@angular/router";
import { RoutesPath } from "@core/enums/routes-path.enum";

export const goToDetails = (id: number, router: Router): void => {
  void router.navigateByUrl(`${RoutesPath.HOME}/${RoutesPath.OFFER}/${id}`);
};