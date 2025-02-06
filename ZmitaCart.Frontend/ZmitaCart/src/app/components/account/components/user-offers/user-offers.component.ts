import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OffersViewComponent } from '@shared/components/offers-view/offers-view.component';

@Component({
  selector: 'pp-user-offers',
  standalone: true,
  imports: [CommonModule, OffersViewComponent],
  templateUrl: './user-offers.component.html',
  styleUrls: ['./user-offers.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserOffersComponent {

}
