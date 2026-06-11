export interface IQuery {
  filterOn?: string;
  filterQuery?: string;
  sortBy?: string;
  isAscending?: boolean;
  pageNum: number;
  pageSize: number;
}
