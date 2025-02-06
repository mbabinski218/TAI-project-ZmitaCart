import { SharedService } from '@shared/services/shared.service';
import { Router } from '@angular/router';
import { UserService } from '@core/services/authorization/user.service';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ppFixPricePipe } from '@shared/pipes/fix-price.pipe';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';
import { MatButtonModule } from '@angular/material/button';
import { RoutesPath } from '@core/enums/routes-path.enum';
import { RoutingService } from '@shared/services/routing.service';
import { BehaviorSubject, Subject, filter, takeUntil, tap } from 'rxjs';
import { GooglePayButtonModule } from '@google-pay/button-angular';
import { OfferSingleService } from '@components/offer-single/api/offer-single.service';
import { ToastMessageService } from '@shared/components/toast-message/services/toast-message.service';

@Component({
  selector: 'pp-offer-price',
  standalone: true,
  imports: [CommonModule, MatIconModule, ppFixPricePipe, MatButtonModule, MatIconModule, GooglePayButtonModule],
  providers: [OfferSingleService],
  templateUrl: './offer-price.component.html',
  styleUrls: ['./offer-price.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class OfferPriceComponent implements OnInit, OnDestroy {

  @Input() details: SingleOffer;

  private onDestroy$ = new Subject<void>();
  currentQuantity = 1;
  currentQuantity$ = new BehaviorSubject<number>(1);
  paymentRequest: google.payments.api.PaymentDataRequest;
  userLogged = false;

  constructor(
    private sharedService: SharedService,
    private userService: UserService,
    private router: Router,
    private routingService: RoutingService,
    private ref: ChangeDetectorRef,
    private offerSingleService: OfferSingleService,
    private toastMessageService: ToastMessageService,
  ) { }

  ngOnInit(): void {
    this.userLogged = this.userService.isAuthenticated();

    this.paymentRequest = {
      apiVersion: 2,
      apiVersionMinor: 0,
      allowedPaymentMethods: [
        {
          type: "CARD",
          parameters: {
            allowedAuthMethods: ["PAN_ONLY", "CRYPTOGRAM_3DS"],
            allowedCardNetworks: ["VISA", "MASTERCARD"]
          },
          tokenizationSpecification: {
            type: "PAYMENT_GATEWAY",
            parameters: {
              gateway: "example",
              gatewayMerchantId: "exampleGatewayMerchantId"
            }
          }
        }
      ],
      merchantInfo: {
        merchantId: String(this.details.user.id),
        merchantName: `${this.details.user.firstName} ${this.details.user.lastName}`
      },
      transactionInfo: {
        totalPriceStatus: "FINAL",
        totalPriceLabel: "Cena",
        totalPrice: '0',
        currencyCode: "PLN",
        countryCode: "PL"
      }
    };

    this.currentQuantity$.asObservable().pipe(
      takeUntil(this.onDestroy$),
    ).subscribe((res) => {
      this.paymentRequest = {
        ...this.paymentRequest,
        transactionInfo: {
          ...this.paymentRequest.transactionInfo,
          totalPrice: String(res * this.details.price),
        }
      };
    });
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  observe(): void {
    if (!this.userService.isAuthenticated())
      return void this.router.navigateByUrl(`${RoutesPath.AUTHENTICATION}/${RoutesPath.LOGIN}`);

    this.sharedService.addToFavourites(this.details.id, this.details.isFavourite).pipe(
      filter((res) => !!res),
      tap(() => this.details.isFavourite = !this.details.isFavourite),
      tap(() => this.ref.detectChanges()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }

  add(val: number) {
    this.currentQuantity += val;
    this.currentQuantity$.next(this.currentQuantity);
  }

  navigateTo(fragment?: string): void {
    this.routingService.navigateTo(`${RoutesPath.HOME}/${RoutesPath.ACCOUNT}`, fragment, { paramName: 'id', value: this.details.id });
  }

  onLoadPaymentData() {
    this.offerSingleService.buy(this.details.id, this.currentQuantity).pipe(
      filter((res) => !!res),
      tap(() => this.toastMessageService.notifyOfSuccess(`Kupiono ${this.details.title}`)),
      tap(() => this.details = { ...this.details, quantity: this.details.quantity - this.currentQuantity }),
      tap(() => { this.currentQuantity = 1; this.currentQuantity$.next(1); }),
      tap(() => { this.details.quantity <= 0 && this.routingService.navigateTo('/'); }),
      tap(() => this.ref.detectChanges()),
      takeUntil(this.onDestroy$),
    ).subscribe();
  }
}