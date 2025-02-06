import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Category } from "@components/add-offer/interfaces/Category";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: 'pp-category-selector',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './category-selector.component.html',
  styleUrls: ['./category-selector.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategorySelectorComponent {
  @Input() item: any;
  @Input() headName: any;
  @Input() children: Category[];
  @Input() categoryIcon?: string;
  @Input() categoryId: Category;
  @Input() headCategory: Category;
  @Input() parentCategory?: Category;
  @Input() isInSubCat: boolean;
  @Output() componentClicked = new EventEmitter<Category>();
  @Output() goBackClicked = new EventEmitter<Category>();

  onClick(id: Category) {
    // this.isInSubCat = true;
    this.componentClicked.emit(id);
  }

  goBack(category: Category) {
    this.goBackClicked.emit(category);
  }

}
