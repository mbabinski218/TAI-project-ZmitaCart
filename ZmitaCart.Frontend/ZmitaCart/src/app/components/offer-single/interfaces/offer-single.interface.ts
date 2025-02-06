export interface SingleOffer {
  id: number,
  title: string,
  description: string,
  price: number,
  quantity: number,
  isAvailable: boolean,
  createdAt: Date,
  condition: string,
  user: {
    id: number,
    email: string,
    firstName: string,
    lastName: string,
    phoneNumber: string,
  },
  address: OfferAddress,
  picturesNames: [
    string,
  ],
  isFavourite: boolean,
}

export interface OfferAddress {
  country: string,
  city: string,
  street: string,
  postalCode: number,
  houseNumber: number,
  apartmentNumber: number,
}