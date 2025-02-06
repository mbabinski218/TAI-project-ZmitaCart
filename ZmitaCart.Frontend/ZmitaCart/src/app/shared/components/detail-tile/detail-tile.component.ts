import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pp-detail-tile',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './detail-tile.component.html',
  styleUrls: ['./detail-tile.component.scss'],
})
export class DetailTileComponent {

  @Input() propertyName: string;
  @Input() icon: string;
}
