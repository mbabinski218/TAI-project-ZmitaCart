import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SingleOffer } from '@components/offer-single/interfaces/offer-single.interface';

@Component({
  selector: 'pp-offer-description',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './offer-description.component.html',
  styleUrls: ['./offer-description.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferDescriptionComponent {
  @Input() details: SingleOffer;
}
