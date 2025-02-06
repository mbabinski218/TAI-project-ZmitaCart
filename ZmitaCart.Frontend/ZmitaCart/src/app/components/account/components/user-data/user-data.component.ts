import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, map } from 'rxjs';
import { UserCredentialsShow } from '@components/account/interfaces/account.interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DetailTileComponent } from '@shared/components/detail-tile/detail-tile.component';
import { stringMask } from '@shared/utils/string-mask';
import { SharedService } from '@shared/services/shared.service';
@Component({
  selector: 'pp-user-data',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, DetailTileComponent],
  templateUrl: './user-data.component.html',
  styleUrls: ['./user-data.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserDataComponent implements OnInit {

  userData$: Observable<UserCredentialsShow[]>;

  constructor(
    private sharedService: SharedService,
  ) { }

  ngOnInit(): void {
    this.userData$ = this.sharedService.getUserData().pipe(
      map((res) => ([
        {
          name: 'Imię',
          value: res.firstName,
          icon: 'badge',
        },
        {
          name: 'Nazwisko',
          value: res.lastName,
          icon: 'person',
        },
        {
          name: 'Email',
          value: res.email,
          icon: 'mail',
        },
        {
          name: 'Numer telefonu',
          value: res.phoneNumber ? stringMask('Numer telefonu', res.phoneNumber) : '-',
          icon: 'dialpad',
        },
        {
          name: 'Państwo',
          value: res.address.country,
          icon: 'flag',
        },
        {
          name: 'Miasto',
          value: res.address.city,
          icon: 'domain',
        },
        {
          name: 'Ulica',
          value: res.address.street,
          icon: 'add_road',
        },
        {
          name: 'Kod pocztowy',
          value: res.address.postalCode ? stringMask('Kod pocztowy', String(res.address.postalCode)) : '-',
          icon: 'markunread_mailbox',
        },
        {
          name: 'Numer domu',
          value: res.address.houseNumber,
          icon: 'house',
        },
        {
          name: 'Numer mieszkania',
          value: res.address.apartmentNumber,
          icon: 'apartment',
        },
      ]))
    );
  }
}
