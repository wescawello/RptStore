import { BehaviorSubject } from "rxjs";
import { IPagerRs } from "./Ipager-rs";
 
export class PagerRs<T> implements IPagerRs<T> {
  Results: T[];
  CurrentPage: number;
  PageCount: number;
  PageSize: number;
  RowCount: number;
  FirstRowOnPage: number;
  LastRowOnPage: number;
  ExtMsg: IDictionary<string>;
  Results$?: BehaviorSubject<T[]>;
  constructor() {
    this.Results$ = new BehaviorSubject<T[]>(null);
    this.ExtMsg = {};
  }
}
 
