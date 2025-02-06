import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pp-allegro-header',
  standalone: true,
  imports: [CommonModule, MatSnackBarModule, RouterModule, MatIconModule],
  templateUrl: './allegro-header.component.html',
  styleUrls: ['./allegro-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AllegroHeaderComponent {

  constructor(
    private _snackBar: MatSnackBar,
  ) { }

  showPrank(): void {
    this._snackBar.open("xdddd nie", '', {
      duration: 4000,
      panelClass: ['prank-snackbar']
    });
  }
}
