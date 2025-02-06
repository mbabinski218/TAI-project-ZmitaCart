export interface SuperiorCategories {
  id: number,
  name: string,
  iconName: string,
  isClickable?: boolean,
}

export interface SubCategories {
  id: number,
  name: string,
  iconName: string,
  children: SubCategories[],
}