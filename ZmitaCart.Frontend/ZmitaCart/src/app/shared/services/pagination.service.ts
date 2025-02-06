import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class PaginationService {

  private currentPage$ = new BehaviorSubject<number>(1);

  private totalPages = 1;
  private currentPage = 1;

  nextPage(side: number): void {
    this.currentPage += side;
    this.applyPageChanged();
  }

  changeToPage(pageNumber: string): void {
    let number = Number(pageNumber);
    if (number > this.totalPages)
      number = this.totalPages;

    if (number < 1)
      number = 1;

    this.currentPage = number;
    this.applyPageChanged();
  }

  getCurrentPage(): Observable<number> {
    return this.currentPage$.asObservable();
  }

  setTotalPages(totalPages: number): void {
    this.totalPages = totalPages;
  }

  private applyPageChanged(): void {
    this.currentPage$.next(this.currentPage);
  }
}
