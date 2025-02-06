import {ChangeDetectionStrategy, Component, EventEmitter, Input, Output} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ConditionType} from "@components/add-offer/interfaces/ConditionType";
import {Condition} from "@core/enums/condition.enum";

@Component({
  selector: 'pp-condition-wrapper',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './condition-wrapper.component.html',
  styleUrls: ['./condition-wrapper.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConditionWrapperComponent {
  @Input() items: ConditionType[];
  @Input() condition: number;
  @Output() componentClicked = new EventEmitter<Condition>();
  @Input() selectedIndex: number;

  selectItem(index: Condition) {
    this.selectedIndex = index;
    this.condition = this.items[index].condition;
    // this.conditionIndex = index;
    // console.log(this.conditionIndex);
    this.componentClicked.emit(this.condition);
  }

}
