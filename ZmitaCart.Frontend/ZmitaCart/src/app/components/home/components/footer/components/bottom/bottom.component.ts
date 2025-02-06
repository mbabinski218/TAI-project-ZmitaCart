import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pp-bottom',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './bottom.component.html',
  styleUrls: ['./bottom.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BottomComponent {

}
