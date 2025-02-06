import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from '@components/home/components/layout/layout.component';
import { UserService } from '@core/services/authorization/user.service';
import { FooterComponent } from '@components/home/components/footer/footer.component';

@Component({
  selector: 'pp-home',
  standalone: true,
  imports: [CommonModule, RouterModule, LayoutComponent, FooterComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit {

  constructor(
    private userService: UserService,
  ) { }

  ngOnInit(): void { }
}
