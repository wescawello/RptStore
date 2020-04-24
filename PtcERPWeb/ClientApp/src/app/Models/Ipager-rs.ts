import { BehaviorSubject } from "rxjs";

export interface IPagerRs<T> {
  Results: T[];
  CurrentPage: number;
  PageCount: number;
  PageSize: number;
  RowCount: number;
  FirstRowOnPage: number;
  LastRowOnPage: number;
  ExtMsg: IDictionary<string>;
  Results$?: BehaviorSubject<T[]>;
}
