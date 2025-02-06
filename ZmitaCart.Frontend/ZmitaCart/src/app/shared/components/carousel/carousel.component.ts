import { ChangeDetectionStrategy, Component, ElementRef, Input, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'pp-carousel',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CarouselComponent implements AfterViewInit {
  @ViewChild('carousel') carousel: ElementRef;

  @Input() imageWidth: number;
  @Input() imagesToScroll: number;

  leftArrow$ = new BehaviorSubject<boolean>(false);
  rightArrow$ = new BehaviorSubject<boolean>(true);

  carouselElement: HTMLElement;
  scrollWidth: number;

  ngAfterViewInit(): void {
    this.carouselElement = this.carousel.nativeElement as HTMLElement;
    this.scrollWidth = this.carouselElement.scrollWidth - this.carouselElement.clientWidth;

    setTimeout(() => {
      this.checkVisibility();
    }, 1);
  }

  move(side: number): void {
    this.carouselElement.scrollLeft += (this.imageWidth * this.imagesToScroll * side);

    setTimeout(() => {
      this.checkVisibility();
    }, 500);
  }

  checkVisibility(): void {
    const visibleLeft = this.carouselElement.scrollLeft === 0 ? false : true;
    const visibleRight = this.carouselElement.scrollLeft === this.scrollWidth ? false : true;

    this.leftArrow$.next(visibleLeft);
    this.rightArrow$.next(visibleRight);
  }
}
