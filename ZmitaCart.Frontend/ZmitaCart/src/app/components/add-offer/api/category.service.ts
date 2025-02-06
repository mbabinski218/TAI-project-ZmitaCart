import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { SuperiorCategory } from "@components/add-offer/interfaces/SuperiorCategory";
import { environment } from "@env/environment";
import { Api } from "@core/enums/api.enum";
import { Category } from "@components/add-offer/interfaces/Category";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  getAllSupCategories(): Observable<SuperiorCategory[]> {
    return this.http.get<SuperiorCategory[]>(`${environment.httpBackend}${Api.SUPERIOR_CATEGORIES}`);
  }

  getFewBySuperiorId(superiorId: number, childrenCount?: number): Observable<Category[]> {
    const options = { params: { 'superiorId': superiorId, 'childrenCount': childrenCount } };
    return this.http.get<Category[]>(`${environment.httpBackend}${Api.GET_FEW_BY_SUP_ID}`, options);
  }

  getSuperiorsWithChildren(childrenCount?: number): Observable<Category[]> {
    const options = {
      params: { 'childrenCount': childrenCount }
    };
    return this.http.get<Category[]>(`${environment.httpBackend}${Api.GET_SUPERIORS_WITH_CHILDREN}`, options);
  }

  getParentCategory(parentId: number): Observable<Category> {
    const options = {
      params: { 'parentId': parentId }
    };

    return this.http.get<Category>(`${environment.httpBackend}${Api.GET_PARENT_CATEGORY}`, options);
  }
}
