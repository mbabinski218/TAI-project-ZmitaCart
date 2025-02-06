export enum Api {
  REGISTER = '/user/register',
  LOGIN = '/user/login',
  RESEND_EMAIL = '/user/resend-email-confirmation',
  CONFIRM_EMAIL = '/user/confirm-email',
  SUPERIOR_CATEGORIES = '/category/getAllSuperiors',
  GET_FEW_BY_SUP_ID = '/category/getFewBySuperiorId',
  GET_SUPERIORS_WITH_CHILDREN = '/category/getSuperiorsWithChildren',
  GET_PARENT_CATEGORY = '/category/getParentCategory',

  LOGOUT = '/user/logout',
  USER_CREDENTIALS_UPDATE = '/user/updateCredentials',
  USER_CREDENTIALS = '/user',
  USER_OFFERS = '/user/offer',
  LOGS = '/user/log',

  GET_SUPERIOR_CATEGORIES = '/category/getAllSuperiors',
  GET_SUB_CATEGORIES = '/category/getBySuperiorId',
  GET_SUB_CATEGORIES_FEW = '/category/getFewBySuperiorId',
  CATEGORIES_POPULAR = '/category/getMostPopular',

  OFFER = '/offer',
  OFFER_EDIT = '/offer/data/:id',
  OFFER_BOUGHT = '/offer/bought',
  OFFER_FAVOURITES = '/offer/favorites',
  OFFER_FAVOURITES_COUNT = '/offer/favorites/count',
  OFFER_SINGLE = '/offer/:id',
  OFFER_BUY = '/offer/buy',
  OFFER_MAIN = '/offer/byCategoriesName',
  OFFER_ADD_TO_FAVOURITES = '/offer/addToFavorites/:id',

  FILE = '/File?name=',
}
