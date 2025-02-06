import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubCategories } from '@components/home/components/header/interfaces/header.interface';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-subcategories-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './subcategories-menu.component.html',
  styleUrls: ['./subcategories-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SubcategoriesMenuComponent {
  @Input() subcategories: SubCategories[];

  constructor(
    private router: Router,
  ) { }

  openCategory(categoryId: number): void {
    void this.router.navigate([`${RoutesPath.HOME}/${RoutesPath.OFFERS_FILTERED}`], {
      queryParams: { c: categoryId },
    });
  }
}
