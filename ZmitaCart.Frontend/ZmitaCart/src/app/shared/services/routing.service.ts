import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { UserService } from '@core/services/authorization/user.service';

@Injectable({
  providedIn: 'root'
})
export class RoutingService {

  constructor(
    private userService: UserService,
    private router: Router,
  ) { }

  navigateTo(path: string, fragment: string = null, params?: { paramName: string, value: string | number }): void {
    let queryParams = {};
    if (params) {
      const { paramName, value } = params;
      queryParams = { [paramName]: value };
    }

    if (!this.userService.isAuthenticated())
      return void this.router.navigateByUrl(`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`);

    void this.router.navigate([path], {
      fragment: fragment,
      queryParams,
    });
  }
}
