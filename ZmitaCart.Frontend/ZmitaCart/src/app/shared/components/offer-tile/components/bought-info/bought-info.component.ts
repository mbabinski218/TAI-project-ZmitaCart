import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoughtOffer } from '@components/account/interfaces/account.interface';

@Component({
  selector: 'pp-bought-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bought-info.component.html',
  styleUrls: ['./bought-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BoughtInfoComponent {
  @Input() item: BoughtOffer; 
}
