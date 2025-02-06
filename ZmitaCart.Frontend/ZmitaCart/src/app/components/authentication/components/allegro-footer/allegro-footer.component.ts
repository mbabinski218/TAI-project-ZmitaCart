import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pp-allegro-footer',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './allegro-footer.component.html',
  styleUrls: ['./allegro-footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AllegroFooterComponent {

}
