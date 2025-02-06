import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OfferMainService } from '@components/offers-main/api/offers-main.service';
import { Observable, Subject, filter, map, of, switchMap, takeUntil, tap } from 'rxjs';
import { MainOffers, Offers } from '@components/offers-main/interfaces/offers-main.interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CarouselComponent } from '@shared/components/carousel/carousel.component';
import { MatIconModule } from '@angular/material/icon';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { UserService } from '@core/services/authorization/user.service';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { SharedService } from '@shared/services/shared.service';
import { IMAGE_URL } from '@shared/constants/shared.constants';
import { goToDetails } from '@shared/utils/offer-details';

@Component({
  selector: 'pp-offers-main',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, CarouselComponent, MatIconModule, ppFixPricePipe],
  providers: [OfferMainService],
  templateUrl: './offers-main.component.html',
  styleUrls: ['./offers-main.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OffersMainComponent implements OnInit, OnDestroy {

  private onDestroy$ = new Subject<void>();

  offers$: Observable<Offers[]>;
  readonly imageUrl = IMAGE_URL;
  details = goToDetails;

  constructor(
    private offerMainService: OfferMainService,
    private sharedService: SharedService,
    private userService: UserService,
    protected router: Router,
    private ref: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.offers$ = this.offerMainService.getMostPopularCategories().pipe(
      switchMap((res) => this.offerMainService.getOffers(res)),
    );
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  observe(item: MainOffers): void {
    if (!this.userService.isAuthenticated())
      return void this.router.navigateByUrl(`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`);

    this.sharedService.addToFavourites(item.id, item.isFavourite).pipe(
      filter((res) => !!res),
      tap(() => item.isFavourite = !item.isFavourite),
      tap(() => this.ref.detectChanges()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }
}