import { ChangeDetectionStrategy, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, map, switchMap, tap } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GalleryModule } from 'ng-gallery';
import { ImageItem } from 'ng-gallery';
import { LightboxModule } from 'ng-gallery/lightbox';
import { OfferDescriptionComponent } from '@components/offer-single/components/offer-description/offer-description.component';
import { OfferSellerComponent } from '@components/offer-single/components/offer-seller/offer-seller.component';
import { OfferPriceComponent } from '@components/offer-single/components/offer-price/offer-price.component';
import { OfferDeliveryComponent } from '@components/offer-single/components/offer-delivery/offer-delivery.component';
import { stringMask } from '@shared/utils/string-mask';
import { SharedService } from '@shared/services/shared.service';
import { IMAGE_URL } from '@shared/constants/shared.constants';

@Component({
  selector: 'pp-offer-single',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, OfferPriceComponent, OfferDeliveryComponent,
    GalleryModule, LightboxModule, OfferDescriptionComponent, OfferSellerComponent],
  templateUrl: './offer-single.component.html',
  styleUrls: ['./offer-single.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class OfferSingleComponent implements OnInit {

  details$: Observable<SingleOffer>;
  images: ImageItem[] = [];

  readonly imageUrl = IMAGE_URL;

  constructor(
    private sharedService: SharedService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.details$ = this.route.params.pipe(
      map(({ id }) => id as string),
      switchMap((id) => this.sharedService.getOffer(id)),
      tap((res) => {
        res.picturesNames?.forEach((name) => {
          const wholeName = this.imageUrl + name;
          this.images.push(new ImageItem({ src: wholeName, thumb: wholeName }));
        });
      }),
      map((res) => {
        return {
          ...res,
          user: {
            ...res.user,
            phoneNumber: stringMask('Numer telefonu', res.user.phoneNumber),
          }
        };
      })
    );
  }
}
