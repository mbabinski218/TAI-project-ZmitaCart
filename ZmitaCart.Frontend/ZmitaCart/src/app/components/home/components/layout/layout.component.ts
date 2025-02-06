import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '@components/home/components/header/header.component';
import { OverlayService } from '@core/services/overlay/overlay.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'pp-layout',
  standalone: true,
  imports: [CommonModule, HeaderComponent],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LayoutComponent implements OnInit {

  isShowCategories$: Observable<boolean>;

  constructor(
    private overlayService: OverlayService
  ) { }

  ngOnInit(): void {
    this.isShowCategories$ = this.overlayService.getState();
  }
  
}
