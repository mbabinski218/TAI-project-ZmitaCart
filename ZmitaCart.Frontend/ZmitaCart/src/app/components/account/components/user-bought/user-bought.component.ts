import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OffersViewComponent } from '@shared/components/offers-view/offers-view.component';

@Component({
  selector: 'pp-user-bought',
  standalone: true,
  imports: [CommonModule, OffersViewComponent],
  templateUrl: './user-bought.component.html',
  styleUrls: ['./user-bought.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserBoughtComponent {

}
