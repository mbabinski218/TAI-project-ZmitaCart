import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OffersViewComponent } from '@shared/components/offers-view/offers-view.component';

@Component({
  selector: 'pp-user-favourites',
  standalone: true,
  imports: [CommonModule, OffersViewComponent],
  templateUrl: './user-favourites.component.html',
  styleUrls: ['./user-favourites.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserFavouritesComponent  {

}
