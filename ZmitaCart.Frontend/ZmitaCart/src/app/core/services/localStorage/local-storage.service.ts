import { Injectable } from '@angular/core';
import { KeyStorage } from '@core/enums/key-storage.enum';
import { Nullable } from '@core/types/nullable';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  public setItem<T>(key: KeyStorage, value: T): void {
    localStorage.setItem(key, JSON.stringify(value));
  }

  public getItem<T>(key: KeyStorage): T {
    const retrievedObject = localStorage.getItem(key);

    if (retrievedObject === null) {
      return {} as T;
    }

    return JSON.parse(retrievedObject) as T;
  }

  public removeItem(key: KeyStorage): void {
    localStorage.removeItem(key);
  }

  public clear(): void {
    localStorage.clear();
  }

  public key(index: number): Nullable<string> {
    return localStorage.key(index);
  }

}
