import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { USEFUL, LAW } from '@components/home/components/footer/components/top/constants/footer-links.const';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pp-top',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './top.component.html',
  styleUrls: ['./top.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TopComponent {

  readonly USEFUL = USEFUL;
  readonly LAW = LAW;

}
