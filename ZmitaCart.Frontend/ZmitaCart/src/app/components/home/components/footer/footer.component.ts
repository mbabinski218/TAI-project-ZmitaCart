import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopComponent } from '@components/home/components/footer/components/top/top.component';
import { BottomComponent } from '@components/home/components/footer/components/bottom/bottom.component';

@Component({
  selector: 'pp-footer',
  standalone: true,
  imports: [CommonModule, TopComponent, BottomComponent],
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FooterComponent {

}
