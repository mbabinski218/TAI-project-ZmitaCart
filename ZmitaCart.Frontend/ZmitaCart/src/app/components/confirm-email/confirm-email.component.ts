import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, from, map, switchMap, tap } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GalleryModule } from 'ng-gallery';
import { LightboxModule } from 'ng-gallery/lightbox';
import { OfferDescriptionComponent } from '@components/offer-single/components/offer-description/offer-description.component';
import { OfferSellerComponent } from '@components/offer-single/components/offer-seller/offer-seller.component';
import { OfferPriceComponent } from '@components/offer-single/components/offer-price/offer-price.component';
import { OfferDeliveryComponent } from '@components/offer-single/components/offer-delivery/offer-delivery.component';
import { PaginationService } from '@shared/services/pagination.service';
import { HeaderStateService } from '@core/services/header-state/header-state.service';
import { MatIconModule } from '@angular/material/icon';
import { LogComponent } from '@shared/components/log/log.component';
import { OverlayService } from '@core/services/overlay/overlay.service';
import { Nullable } from '@core/types/nullable';
import { OffersFilteredService } from '@components/offers-filtered/api/offers-filtered.service';
import { OfferTileComponent } from '@shared/components/offer-tile/offer-tile.component';
import { OffersFiltersComponent } from '@shared/components/offers-view/offers-filters/offers-filters.component';
import { FormsModule } from '@angular/forms';
import { AuthenticationService } from '@components/authentication/api/authentication.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'pp-confirm-email',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, OfferPriceComponent, OfferDeliveryComponent, GalleryModule, LightboxModule, OfferDescriptionComponent,
    OfferSellerComponent, MatIconModule, LogComponent, FormsModule],
  providers: [PaginationService, OffersFilteredService, OfferTileComponent, OffersFiltersComponent, AuthenticationService],
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class ConfirmEmailComponent implements OnInit {
  text: string = "";

  constructor(    
    private headerStateService: HeaderStateService,
    protected paginationService: PaginationService,
    protected overlayService: OverlayService,
    protected authenticationService: AuthenticationService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.overlayService.setState(false);
    this.headerStateService.setShowSearch(false);

    this.route.queryParams.subscribe((params) => {
      const token = encodeURIComponent(params.Token);
      const id = params.Id;
      console.log(token, id);
      this.authenticationService.confirmEmail(id, token)
        .subscribe({
            next: (res) => {
              if (res) {
                this.text = 'Email został potwierdzony';
              } else {
                this.text = 'Błąd podczas potwierdzania emaila';
              }
              this.cdr.detectChanges();
            },
            error: (err) => {
              this.text = 'Błąd podczas potwierdzania emaila'
              this.cdr.detectChanges();
            },
        });
    });
  }
}
