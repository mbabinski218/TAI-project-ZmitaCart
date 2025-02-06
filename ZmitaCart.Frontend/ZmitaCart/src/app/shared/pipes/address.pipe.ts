import { Pipe, PipeTransform } from '@angular/core';
import { OfferAddress } from '@components/offer-single/interfaces/offer-single.interface';

@Pipe({
  name: 'ppAddress',
  standalone: true,
})
export class ppAddressPipe implements PipeTransform {
  transform(value: OfferAddress): string {
    const { city, street, houseNumber, apartmentNumber } = value;

    let address = "";

    if (city) {
      address += city;
    }

    if (street) {
      address += ", ul." + street;
    }

    if (houseNumber) {
      address += " " + String(houseNumber);
    }

    if (apartmentNumber) {
      address += "/" + String(apartmentNumber);
    }

    return address;
  }
}
