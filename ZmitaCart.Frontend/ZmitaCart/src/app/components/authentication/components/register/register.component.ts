import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, Validators, ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { emailPattern, passwordPattern } from '@shared/patterns/valid.pattern';
import { mustMatch } from '@shared/utils/must-match';
import { InputComponent } from '@shared/components/input/input.component';
import { MatIconModule } from '@angular/material/icon';
import { AuthenticationService } from '@components/authentication/api/authentication.service';
import { Subject, filter, takeUntil, tap } from 'rxjs';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-register',
  standalone: true,
  imports: [CommonModule, FormsModule, InputComponent, ReactiveFormsModule, MatIconModule],
  providers: [AuthenticationService],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent implements OnDestroy {

  private onDestroy$ = new Subject<void>();
  
  form = new FormGroup({
    email: new FormControl('', Validators.compose([Validators.required, Validators.pattern(emailPattern)])),
    firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(35)])),
    lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(35)])),
    password: new FormControl('', Validators.compose([Validators.required, Validators.pattern(passwordPattern)])),
    confirmedPassword: new FormControl('', Validators.required),
  },
    {
      updateOn: 'change',
      validators: mustMatch('password', 'confirmedPassword'),
    }
  );

  passwordVisibility = {
    password: false,
    confirmedPassword: false,
  };

  constructor(
    private authenticationService: AuthenticationService,
    private toastMessageService: ToastMessageService,
    private router: Router,
  ) { }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  togglePasswordVisibility(inputName: 'password' | 'confirmedPassword') {
    this.passwordVisibility[inputName] = !this.passwordVisibility[inputName];
  }

  register(): void {
    if (this.form.valid)
      this.authenticationService.register(this.form.value).pipe(
        filter((res) => !!res),
        tap(() => this.toastMessageService.notifyOfSuccess('Konto zostało utworzone')),
        tap(() => this.router.navigate([`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`])),
        takeUntil(this.onDestroy$),
      ).subscribe();
  }
}
