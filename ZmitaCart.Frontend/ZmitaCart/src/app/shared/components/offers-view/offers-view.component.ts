import { ChangeDetectionStrategy, Component, ViewEncapsulation, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, map, switchMap, tap } from 'rxjs';
import { BoughtOffers } from '@components/account/interfaces/account.interface';
import { AccountService } from '@components/account/api/account.service';
import { PaginationService } from '@shared/services/pagination.service';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { OfferTileComponent } from '@shared/components/offer-tile/offer-tile.component';
import { OffersFilteredService } from '@components/offers-filtered/api/offers-filtered.service';
import { ActivatedRoute } from '@angular/router';
import { OffersFiltersComponent } from '@shared/components/offers-view/offers-filters/offers-filters.component';
import { INIT_OFFERS_FORM_CONST, MyForm } from '@shared/components/offers-view/offers-filters/interfaces/offers-view.interface';

@Component({
  selector: 'pp-offers-view',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatProgressSpinnerModule, OfferTileComponent, OffersFiltersComponent],
  providers: [AccountService, PaginationService, OffersFilteredService],
  templateUrl: './offers-view.component.html',
  styleUrls: ['./offers-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class OffersViewComponent implements OnInit {

  @Input() header: string;
  @Input() noDataText: string;
  @Input() origin: 'favourites' | 'bought' | 'filtered' | 'user-offers';

  OFFERS_FORM = INIT_OFFERS_FORM_CONST;

  offers$: Observable<BoughtOffers>;

  totalPages = 1;
  currentPage = 1;
  areAnyOffers = false;
  canGo = {
    right: false,
    left: false,
  };

  constructor(
    private accountService: AccountService,
    private offersFilteredService: OffersFilteredService,
    private route: ActivatedRoute,
    protected paginationService: PaginationService,
  ) { }

  ngOnInit(): void {
    this.offers$ = this.setData();
  }

  private setData(): Observable<BoughtOffers> {
    if (this.origin === 'bought') {
      return this.paginationService.getCurrentPage().pipe(
        switchMap((res) => this.accountService.getBought(res)),
        tap((res) => this.paginationService.setTotalPages(res.totalPages)),
        tap((res) => this.setAll(res)),
      );
    }

    return this.paginationService.getCurrentPage().pipe(
      switchMap((res) => {
        switch (this.origin) {
          case "favourites": {
            return this.accountService.getFavourites(res);
          }
          case "filtered": {
            return this.getFilteredOffers(res);
          }
          case "user-offers": {
            return this.accountService.getUserOffers(res);
          }
        }
      }),
      map((res) => {
        return {
          ...res,
          items: res.items.map(item => ({
            offer: {
              ...item,
            },
          })),
        };
      }),
      tap((res) => this.paginationService.setTotalPages(res.totalPages)),
      tap((res) => this.setAll(res)),
    );
  }

  filterData(form: MyForm) {
    this.OFFERS_FORM.patchValue(form);
    this.offers$ = this.setData();
  }

  private getFilteredOffers(pageNumber: number) {
    return this.route.queryParams.pipe(
      map(({ c, i }) => ({ c, i })),
      switchMap(({ c, i }) => this.offersFilteredService.getOffers(c, i, pageNumber, this.OFFERS_FORM)),
    );
  }

  private setAll(data: BoughtOffers): void {
    this.totalPages = data.totalPages;
    this.currentPage = data.pageNumber;
    this.areAnyOffers = !!data.totalCount;
    this.canGo = { right: data.hasNextPage, left: data.hasPreviousPage };
  }
}
