export interface Category {
  id: number,
  name: string,
  parentId?: number;
  children?: Category[]
  iconName?: string;
}

export interface OfferToEdit {
  title: string,
  description: string,
  price: number,
  quantity: number,
  condition: string,
  categoryId: number,
  isAvailable: boolean,
  picturesNames: string[]
}
