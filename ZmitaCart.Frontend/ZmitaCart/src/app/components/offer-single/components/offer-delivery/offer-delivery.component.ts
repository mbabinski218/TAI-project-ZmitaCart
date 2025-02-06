import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'pp-offer-delivery',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './offer-delivery.component.html',
  styleUrls: ['./offer-delivery.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferDeliveryComponent {

}
