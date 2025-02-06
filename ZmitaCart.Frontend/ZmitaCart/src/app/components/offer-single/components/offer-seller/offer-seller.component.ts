import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';
import { MatIconModule } from '@angular/material/icon';
import { ppAddressPipe } from '@shared/pipes/address.pipe';

@Component({
  selector: 'pp-offer-seller',
  standalone: true,
  imports: [CommonModule, MatIconModule, ppAddressPipe],
  templateUrl: './offer-seller.component.html',
  styleUrls: ['./offer-seller.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferSellerComponent {
  @Input() details: SingleOffer;
}
