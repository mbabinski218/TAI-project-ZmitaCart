import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ToastMessageComponent } from '@shared/components/toast-message/toast-message.component';

@Injectable({
  providedIn: 'root'
})
export class ToastMessageService {

  constructor(private _snackBar: MatSnackBar) { }

  notifyOfSuccess(data: string): void {
    this._snackBar.openFromComponent(ToastMessageComponent, {
      duration: 4000,
      data: data,
      panelClass: 'success-snackbar',
    });
  }

  notifyOfError(error: string): void {
    this._snackBar.openFromComponent(ToastMessageComponent, {
      duration: 4000,
      data: error,
      panelClass: 'error-snackbar',
    });
  }
}
