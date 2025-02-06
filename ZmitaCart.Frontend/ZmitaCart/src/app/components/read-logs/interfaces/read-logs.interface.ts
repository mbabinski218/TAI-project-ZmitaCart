export interface Log {
  id: number,
  timestamp: Date,
  action: string,
  isSuccess: boolean,
  details: string,
  ipAddress: string,
  userAgent: string,
  userId?: number,
  userEmail?: string
}

interface Pagination {
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}

export interface Logs extends Pagination {
  items: Log[],
}
