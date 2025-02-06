import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AllegroFooterComponent } from '@components/authentication/components/allegro-footer/allegro-footer.component';
import { AllegroHeaderComponent } from '@components/authentication/components/allegro-header/allegro-header.component';

@Component({
  selector: 'pp-authentication',
  standalone: true,
  imports: [CommonModule, RouterModule, AllegroFooterComponent, AllegroHeaderComponent],
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthenticationComponent {

}
