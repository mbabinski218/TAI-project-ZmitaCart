import { Subject, takeUntil } from 'rxjs';
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TokenData } from '@components/authentication/interfaces/authentication-interface';
import { MatMenuModule } from '@angular/material/menu';
import { RouterModule } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { MatIconModule } from '@angular/material/icon';
import { ClickOutsideDirective } from '@shared/directives/click-outside.directive';
import { UserService } from '@core/services/authorization/user.service';
import { isEmpty } from 'lodash';
import { HeaderService } from '@components/home/components/header/api/header.service';

@Component({
  selector: 'pp-login-menu',
  standalone: true,
  imports: [CommonModule, MatMenuModule, RouterModule, MatIconModule, ClickOutsideDirective],
  providers: [HeaderService],
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginMenuComponent implements OnInit, OnDestroy {

  private onDestroy$ = new Subject<void>();

  readonly LOGIN_LINK = `/${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`;
  readonly RoutesPath = RoutesPath;

  opened = false;
  userData: TokenData;
  emptyData: boolean;

  constructor(
    private userService: UserService,
    private headerService: HeaderService,
  ) { }

  ngOnInit(): void {
    this.userData = this.userService.userAuthorization();
    this.emptyData = isEmpty(this.userData);
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  logout(): void {
    this.headerService.logout().pipe(
      takeUntil(this.onDestroy$),
    ).subscribe(() => { this.userService.logout(); window.location.reload(); });
  }
}
