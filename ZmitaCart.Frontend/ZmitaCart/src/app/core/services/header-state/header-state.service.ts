import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HeaderStateService {

  private isShowSearch$ = new BehaviorSubject<boolean>(true);
  private isShowAddOfferButton$ = new BehaviorSubject<boolean>(true);

  setShowSearch(isShow: boolean): void {
    this.isShowSearch$.next(isShow);
  }

  getShowSearch(): Observable<boolean> {
    return this.isShowSearch$.asObservable();
  }

  setShowAddOfferButton(isShow: boolean): void {
    this.isShowAddOfferButton$.next(isShow);
  }

  getShowAddOfferButton(): Observable<boolean> {
    return this.isShowAddOfferButton$.asObservable();
  }

  resetHeaderState(): void {
    this.isShowSearch$.next(true);
    this.isShowAddOfferButton$.next(true);
  }
}
