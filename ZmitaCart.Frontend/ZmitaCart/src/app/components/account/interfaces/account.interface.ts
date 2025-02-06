export interface CredentialsForm {
  phoneNumber: string,
  address: Address,
}

export interface UserCredentials {
  email: string,
  firstName: string,
  lastName: string,
  phoneNumber: string,
  address: Address,
}

export interface UserCredentialsShow {
  name: string,
  value: string | number,
  icon: string,
}

export interface AccountOffers extends Pagination {
  items: OfferItem[],
}

export interface BoughtOffers extends Pagination {
  items: BoughtOffer[],
}


export interface OfferItem {
  id: number,
  title: string,
  price: number,
  city: string,
  condition: string,
  quantity: number,
  imageName: string,
  isFavourite: boolean,
  isAvailable: boolean,
  authorName: string,
  authorEmail: string,
}

export interface BoughtOffer {
  offer: OfferItem,
  boughtAt?: Date,
  boughtQuantity?: number,
  totalPrice?: number,
}

interface Pagination {
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}

interface Address {
  country: string,
  city: string,
  street: string,
  postalCode: number,
  houseNumber: number,
  apartmentNumber: number,
}