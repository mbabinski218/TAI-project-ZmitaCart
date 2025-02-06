import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubCategories, SuperiorCategories } from '@components/home/components/header/interfaces/header.interface';
import { MatIconModule } from '@angular/material/icon';
import { HeaderService } from '@components/home/components/header/api/header.service';
import { Observable } from 'rxjs';
import { SubcategoriesMenuComponent } from '@components/home/components/header/components/categories-menu/subcategories-menu/subcategories-menu.component';
import { Router } from '@angular/router';
import { RoutesPath } from '@core/enums/routes-path.enum';

@Component({
  selector: 'pp-categories-menu',
  standalone: true,
  imports: [CommonModule, MatIconModule, SubcategoriesMenuComponent],
  providers: [HeaderService],
  templateUrl: './categories-menu.component.html',
  styleUrls: ['./categories-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategoriesMenuComponent {

  @Input() categories: SuperiorCategories[];

  subcategoriesLoaded: SubCategories[];

  subcategories$: Observable<SubCategories>;

  constructor(
    private headerService: HeaderService,
    private router: Router,
  ) { }

  openSubCategories(category: SuperiorCategories): void {
    this.subcategories$ = this.headerService.getSubCategories(category.id);
  }

  openCategory(categoryId: number): void {
    void this.router.navigate([`${RoutesPath.HOME}/${RoutesPath.OFFERS_FILTERED}`], {
      queryParams: { c: categoryId },
    });
  }
}
