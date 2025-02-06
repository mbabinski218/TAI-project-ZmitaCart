import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, from, map, switchMap, tap } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GalleryModule } from 'ng-gallery';
import { LightboxModule } from 'ng-gallery/lightbox';
import { OfferDescriptionComponent } from '@components/offer-single/components/offer-description/offer-description.component';
import { OfferSellerComponent } from '@components/offer-single/components/offer-seller/offer-seller.component';
import { OfferPriceComponent } from '@components/offer-single/components/offer-price/offer-price.component';
import { OfferDeliveryComponent } from '@components/offer-single/components/offer-delivery/offer-delivery.component';
import { ReadLogsService } from './api/read-logs.service';
import { Logs } from './interfaces/read-logs.interface';
import { PaginationService } from '@shared/services/pagination.service';
import { HeaderStateService } from '@core/services/header-state/header-state.service';
import { MatIconModule } from '@angular/material/icon';
import { LogComponent } from '@shared/components/log/log.component';
import { OverlayService } from '@core/services/overlay/overlay.service';
import { Nullable } from '@core/types/nullable';
import { OffersFilteredService } from '@components/offers-filtered/api/offers-filtered.service';
import { OfferTileComponent } from '@shared/components/offer-tile/offer-tile.component';
import { OffersFiltersComponent } from '@shared/components/offers-view/offers-filters/offers-filters.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'pp-read-logs',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, OfferPriceComponent, OfferDeliveryComponent, GalleryModule, LightboxModule, OfferDescriptionComponent,
    OfferSellerComponent, MatIconModule, LogComponent, FormsModule],
  providers: [PaginationService, ReadLogsService, OffersFilteredService, OfferTileComponent, OffersFiltersComponent],
  templateUrl: './read-logs.component.html',
  styleUrls: ['./read-logs.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class ReadLogsComponent implements OnInit, OnDestroy {

  @Input() header: string;
  @Input() noDataText: string;

  searchText: Nullable<string> = null;
  status: Nullable<boolean> = null;
  dateFrom: Nullable<string> = null;
  dateTo: Nullable<string> = null;

  totalPages = 1;
  currentPage = 1;
  areAnyLogs = false;
  canGo = {
    right: false,
    left: false,
  };

  logs$: Observable<Logs>;

  constructor(
    private readLogsService: ReadLogsService,
    private headerStateService: HeaderStateService,
    protected paginationService: PaginationService,
    protected overlayService: OverlayService,
  ) { }

  ngOnInit(): void {
    this.overlayService.setState(false);
    this.headerStateService.setShowSearch(false);
    this.logs$ = this.setData();
  }

  ngOnDestroy(): void {
    this.headerStateService.resetHeaderState();
  }

  private setData(): Observable<Logs> {
    return this.paginationService.getCurrentPage().pipe(
      switchMap((res) => this.readLogsService.getLogs(this.searchText, this.status, this.dateFrom, this.dateTo, res)),
      tap((res) => this.paginationService.setTotalPages(res.totalPages)),
      tap((res) => this.setPaginationData(res)),
    );
  }

  private setPaginationData(data: Logs): void {
    this.totalPages = data.totalPages;
    this.currentPage = data.pageNumber;
    this.areAnyLogs = !!data.totalCount;
    this.canGo = { right: data.hasNextPage, left: data.hasPreviousPage };
  }

  public applyFilters(): void {
    this.paginationService.changeToPage("1");
    this.totalPages = 1;
    this.currentPage = 1;
    this.areAnyLogs = false;
    this.canGo = {
      right: false,
      left: false,
    };

    this.logs$ = this.setData();
  }

  resetFilters(): void {
    this.searchText = null;
    this.status = null;
    this.dateFrom = null;
    this.dateTo = null;

    this.logs$ = this.setData();
  }
}
