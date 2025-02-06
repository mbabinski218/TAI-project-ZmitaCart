import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferItem } from '@components/account/interfaces/account.interface';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AreYouSureDialogComponent } from '@shared/components/are-you-sure-dialog/are-you-sure-dialog.component';
import { NoopScrollStrategy } from '@angular/cdk/overlay';
import { RoutingService } from '@shared/services/routing.service';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'pp-edit-delete',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatDialogModule],
  templateUrl: './edit-delete.component.html',
  styleUrls: ['./edit-delete.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EditDeleteComponent implements OnDestroy {

  @Input() item: OfferItem;
  @Output() deleteOffer = new EventEmitter<boolean>();

  private onDestroy$ = new Subject<void>();

  constructor(
    private dialog: MatDialog,
    private routingService: RoutingService,
  ) { }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AreYouSureDialogComponent, {
      disableClose: true,
      scrollStrategy: new NoopScrollStrategy(),
    });

    dialogRef.afterClosed().pipe(
      takeUntil(this.onDestroy$),
    ).subscribe((res) => {
      if (res === 'yes')
        this.deleteOffer.emit(true);
    });
  }

  goToEdit(): void {
    this.routingService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.ADD_OFFER}`, null, { paramName: 'id', value: this.item.id });
  }
}
