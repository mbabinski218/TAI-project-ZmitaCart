export interface Offers {
  name: string,
  data: MainOffers[],
}

export interface MainOffers {
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