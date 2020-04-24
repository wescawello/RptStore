import { HttpClient } from "@angular/common/http";
import { AuthorizeService } from "../../api-authorization/authorize.service";
import { AlertService } from "../shared/alert.service";
import { BehaviorSubject } from "rxjs";
import { skip, take, catchError, map, finalize } from "rxjs/operators";
import { IPagerRs } from "../Models/Ipager-rs";
import { PagerRs } from "../Models/pager-rs";
import { TablayoutService } from "./tablayout.service";
import { isNull } from "util";

export class Resful<T,X> {
  qing: boolean;
  pagerObj: IPagerRs<T> = new PagerRs<T>();
  qa$ = new BehaviorSubject<Qa<X>>({
    page: 1, pagesize: 10, queymodel: {} as X ,
    sort: {}
  });
  qorder = {};
  http: HttpClient;
  baseUrl: string;
  pagermaxSize = 10;
  alterr = (err: { status: number; error: string }) => {
    if (err.status === 500) {
      console.log(err);
      this.alts.add(err.error, "danger");
    }
    throw err;
  }
  constructor(private tl: TablayoutService, private authorizeService: AuthorizeService, public alts: AlertService, public pathPrefix: string, public syncq  = false) {
    this.http = tl.sharehttp;
    this.baseUrl = tl.sharebaseUrl;
    this.qa$.pipe(skip(1)).subscribe(async qa => {
      const mq = { ...qa.queymodel, sort: qa.sort };
      if (!this.qing) {
        console.log(mq);
        await this.query(mq, qa.pagesize, qa.page).toPromise();
      }
      else {
        alts.add("Query processing", "danger");
      }
    });
    if (syncq) {
      tl.syncq$.pipe(skip(1)).subscribe(() => {
        this.qa$.next({ ...this.qa$.getValue()});
      });
    }
 
    console.log(`Resful is constructed by ${pathPrefix}`);
  }
 


  query(q, s, p) {
    this.qing = true;
    return this.http.post<IPagerRs<T>>(`${this.baseUrl}api/Query/${this.pathPrefix}/${s}/${p}`, q).pipe(take(1), catchError(this.alterr), map(o => {
      this.pagerObj = { ... this.pagerObj, ...o };
      this.pagerObj.Results$.next(o.Results);
      return this.pagerObj;
    }), finalize(() => {
      console.log("set ffff");
      this.qing = false;
    }));
  }
  preQuery(q) {
    this.qa$.next({ ...this.qa$.getValue(), queymodel: q, page: 1 });
  }
  chPage(p: number) {
    const qcurr = this.qa$.getValue();
    if (qcurr.page !== p) {
      this.qa$.next({ ...qcurr, page: p });
    }
  }
  chPageSize(s: number) {
    this.qa$.next({ ...this.qa$.getValue(), pagesize: s });
  }

  async add(o: IC) {
    if (!this.qing) {
      this.qing = true;
      o.CreateId = await this.getuid();
      o.CreateDate = o.CreateDate || new Date();
      const add = await this.http.post(`${this.baseUrl}api/${this.pathPrefix}`, o).pipe(take(1), catchError(this.alterr), finalize(() => {
        this.qing = false;
        this.tl.onSelectTab(null, 0);
        this.tl.syncq$.next({});
      })
      ).toPromise();
      return add;
    } else {
      this.alts.add("Add Fail processing", "danger");
    }
  }
  


  async find(tbid: string) {
 
    if (!this.qing) {
      this.qing = true;
      return await this.http.get(`${this.baseUrl}api/${this.pathPrefix}/${tbid}`).pipe(take(1), catchError(this.alterr),map(o => {
        Object.keys(o).forEach(k => {
          if (k.endsWith("Date") && !isNull(o[k])) {
            o[k] = new Date(o[k]);
          }
        });
        return o;


      }), finalize(() => {
        this.qing = false;
      })).toPromise();
    }
    else {
      this.alts.add("Find Fail processing", "danger");
    }
  }
  async mod(o, tbid) {
    o.UpdateDate = new Date();
    o.UpdateId = await this.getuid();
    console.log(o.UpdateId);
    if (!this.qing) {
      this.qing = true;
      this.http.put(`${this.baseUrl}api/${this.pathPrefix}/${tbid}`, o).pipe(take(1), catchError(this.alterr), finalize(() => {
        this.qing = false;
      })
      ).subscribe(_ => {
        if (this.syncq) {
          this.tl.syncq$.next({});
        }
        this.tl.onSelectTab(null, 0);
      }, this.alterr
      );
    }
    else {
      this.alts.add("Update Fail processing", "danger");
    }
  }
  async del(o) {
    if (!this.qing) {
      this.qing = true;
      return await this.http.delete(`${this.baseUrl}api/${this.pathPrefix}/${o}`).pipe(take(1), catchError(this.alterr),
        finalize(() => {
        this.qing = false;
        this.tl.syncq$.next({});
      })
      ).toPromise() as IdelCheck;
    } else {
      this.alts.add("Del Fail processing", "danger");
    }
  }
  async getuid() {
    return await this.authorizeService.getUserIdFromStorage().toPromise();
  }
  setOrder(a, b) {
    //console.log([a, b]);
    if (!b) {
      if (Object.keys(this.qorder).includes(a)) {
        delete this.qorder[a]
      }
    } else {
      this.qorder[a] = b;
    }
    this.chsort(this.qorder)
  }
  chsort(o) {
    this.qa$.next({ ...this.qa$.getValue(), sort: o });
  }
}
class Qa<O> {
  queymodel: O;
  page: number;
  pagesize: number;
  sort: IDictionary<string>;
}
 
