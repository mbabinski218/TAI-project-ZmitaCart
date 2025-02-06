import { Injectable } from '@angular/core';
import { Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { environment } from "@env/environment";
import { Api } from "@core/enums/api.enum";
import { OfferToEdit } from '@components/add-offer/interfaces/Category';

@Injectable({
  providedIn: 'root'
})
export class OfferService {
  formData: FormData;

  constructor(private http: HttpClient) {
  }

  createOffer(title: string, desc: string, price: number, quantity: number, condition: string, categoryId: number, pics: File[]): Observable<number> {
    this.formData = new FormData();
    this.formData.append('title', title);
    this.formData.append('description', desc);
    this.formData.append('price', price.toString());
    this.formData.append('quantity', quantity.toString());
    this.formData.append('condition', condition);
    this.formData.append('categoryId', categoryId.toString());

    if (pics?.length > 0) {
      Array.from(pics).forEach(f => this.formData.append('pictures', f));
    }

    return this.http.post<number>(`${environment.httpBackend}${Api.OFFER}`, this.formData);
  }

  updateOffer(id: number, title: string, desc: string, price: number, quantity: number, condition: string, isAvailable: boolean, pics: File[]): Observable<number> {
    this.formData = new FormData();
    this.formData.append('id', id.toString());
    this.formData.append('title', title);
    this.formData.append('description', desc);
    this.formData.append('price', price.toString());
    this.formData.append('quantity', quantity.toString());
    this.formData.append('condition', condition);
    this.formData.append('isAvailable', isAvailable.toString());

    if (pics?.length > 0) {
      Array.from(pics).forEach(f => this.formData.append('editedPictures', f));
    }

    return this.http.put<number>(`${environment.httpBackend}${Api.OFFER}`, this.formData);
  }

  goToEditOffer(id: string): Observable<OfferToEdit> {
    return this.http.get<OfferToEdit>(`${environment.httpBackend}${Api.OFFER_SINGLE}`.replace(':id', id));
  }

  restoreOfferImages(imageNames: string[]): Observable<any> {

    const files: File[] = [];
    imageNames?.forEach(name => {
      this.http.get(`${environment.httpBackend}${Api.FILE}${name}`, { responseType: 'blob' }).subscribe(
        res => {
          const file = new File([res], name);
          files.push(file);
        }
      );
    });

    return of(files);
  }
}
