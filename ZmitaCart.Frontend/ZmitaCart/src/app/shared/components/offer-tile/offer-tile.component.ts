import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoughtOffer } from '@components/account/interfaces/account.interface';
import { MatIconModule } from '@angular/material/icon';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { Router } from '@angular/router';
import { FavouriteBuyComponent } from '@shared/components/offer-tile/components/favourite-buy/favourite-buy.component';
import { EditDeleteComponent } from '@shared/components/offer-tile/components/edit-delete/edit-delete.component';
import { OfferSingleService } from '@components/offer-single/api/offer-single.service';
import { Observable, Subject, filter, takeUntil, tap } from 'rxjs';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';
import { BoughtInfoComponent } from '@shared/components/offer-tile/components/bought-info/bought-info.component';
import { IMAGE_URL } from '@shared/constants/shared.constants';
import { goToDetails } from '@shared/utils/offer-details';

@Component({
  selector: 'pp-offer-tile',
  standalone: true,
  imports: [CommonModule, MatIconModule, ppFixPricePipe, FavouriteBuyComponent, EditDeleteComponent, BoughtInfoComponent],
  providers: [OfferSingleService],
  templateUrl: './offer-tile.component.html',
  styleUrls: ['./offer-tile.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OfferTileComponent implements OnDestroy {

  @Input() items: BoughtOffer[];
  @Input() origin: 'favourites' | 'bought' | 'filtered' | 'user-offers';

  readonly imageUrl = IMAGE_URL;
  private onDestroy$ = new Subject<void>();

  siema: Observable<boolean>;
  details = goToDetails;

  constructor(
    protected router: Router,
    private offerSingleService: OfferSingleService,
    private toastMessageService: ToastMessageService,
    private ref: ChangeDetectorRef,
  ) { }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  deleteOffer(item: BoughtOffer) {
    this.offerSingleService.deleteOffer(String(item.offer.id)).pipe(
      filter((res) => !!res),
      tap(() => this.items = this.items.filter((res) => res !== item)),
      tap(() => this.toastMessageService.notifyOfSuccess('Usunięto ofertę')),
      tap(() => this.ref.detectChanges()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }
}
