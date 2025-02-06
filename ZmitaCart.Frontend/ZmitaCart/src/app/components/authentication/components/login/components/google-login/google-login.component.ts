/* eslint-disable @typescript-eslint/ban-ts-comment */
import { ChangeDetectionStrategy, Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CredentialResponse } from 'google-one-tap';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { AuthenticationService } from '@components/authentication/api/authentication.service';
import { GoogleLoginProvider, SocialAuthService, SocialAuthServiceConfig } from '@abacritt/angularx-social-login';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'pp-google-login',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  providers: [AuthenticationService, SocialAuthService, {
    provide: 'SocialAuthServiceConfig',
    useValue: {
      autoLogin: false,
      providers: [
        {
          id: GoogleLoginProvider.PROVIDER_ID,
          provider: new GoogleLoginProvider(
            '267976867905-jk39h0f3jtlpjmtqmr79c2ehd5oq2dig.apps.googleusercontent.com'
          )
        },
      ],
    } as SocialAuthServiceConfig,
  }
  ],
  templateUrl: './google-login.component.html',
  styleUrls: ['./google-login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GoogleLoginComponent implements OnInit, OnDestroy {

  private onDestroy$ = new Subject<void>();

  constructor(
    private router: Router,
    private _ngZone: NgZone,
    private authenticationService: AuthenticationService,
    private authService: SocialAuthService,//musi zostać, inaczej nie działa xdd
  ) { }

  ngOnInit(): void {
    // @ts-ignore
    window.onGoogleLibraryLoad = () => {
      // @ts-ignore
      google.accounts.id.initialize({
        client_id: '267976867905-jk39h0f3jtlpjmtqmr79c2ehd5oq2dig.apps.googleusercontent.com',
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true
      });

      // @ts-ignore
      google.accounts.id.renderButton(
        document.getElementById("googleLoginButton"),
        { theme: "outline", size: "large", text: 'continue_with' }
      );
    };
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  private handleCredentialResponse(response: CredentialResponse) {
    this.authenticationService.googleLogin(response.credential).pipe(
      takeUntil(this.onDestroy$),
    ).subscribe(() => {
      this._ngZone.run(() => {
        void this.router.navigate(['/']);
      });
    }
    );
  }
}