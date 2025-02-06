import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class OverlayService {

  private isShowOverlay$ = new BehaviorSubject<boolean>(false);

  setState(isShow: boolean): void {
    this.isShowOverlay$.next(isShow);
  }

  getState(): Observable<boolean> {
    return this.isShowOverlay$.asObservable();
  }

  resetState(): void {
    this.isShowOverlay$.next(false);
  }

}
