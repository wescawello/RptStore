import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { map, take, skip, catchError } from 'rxjs/operators';
import { PagerRs } from '../Models/pager-rs';
import { IPagerRs } from '../Models/Ipager-rs';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { AlertService } from '../shared/alert.service';
import { Resful } from './resful';
 
 
 

@Injectable({
  providedIn: 'root',
  
})
export class TablayoutService   {
   
  private actionurl: string;
  public qorder = {};
  targetQa$: BehaviorSubject<qa>;
  syncq$ = new BehaviorSubject({});

  qa$ = new BehaviorSubject<qa>({
    page:1, pagesize:10, queymodel:{},
    sort:{}
  });
  qAdd$ = new BehaviorSubject<qa>({
    page: 1, pagesize: 10, queymodel: {},
    sort: {}
  });
  qSaleAdd$ = new BehaviorSubject<qa>({
    page: 1, pagesize: 10, queymodel: {},
    sort: {}
  });

  preAdd$ = new BehaviorSubject<any>({});

  userIdName$ = new BehaviorSubject<IDictionary<string>>({ });
    actionqAddurl: string;
  oduserIdName(us: string[]) {
    let un = this.userIdName$.getValue();
    us.forEach(s => {
      if (s && !Object.keys(un).includes(s)) {
        return this.http.get<string>(`${this.baseUrl}api/Query/UserIdName/${s}`).pipe(take(1)).subscribe(n => {
          un[s] = n;
          this.userIdName$.next(un);
        });
      }
    });

  }

  tabNames$ = new BehaviorSubject({
    query: "",
    add: "",
    edit:""
  });


  pagermaxSize = 10;
 
  pagerObj=new PagerRs<any>();
  pagerAddObj = new PagerRs<any>();
  qing: boolean;

  initAdd(s: string) {
    this.actionqAddurl = s;
    this.qAdd$ = new BehaviorSubject<qa>({
      page: 1, pagesize: 10, queymodel: {},
      sort: {}
    });
    this.qAdd$.pipe(skip(1)).subscribe(async qa => {
      const mq = { ...qa.queymodel, sort: qa.sort };
      if (!this.qing) {
        console.log(qa);
        await this.queryAdd(mq, qa.pagesize, qa.page).toPromise();

      }
    });
    this.targetQa$ = this.qAdd$;
  }

  
  init(s): void {
    this.activatedTabIndex = 0;
    this.addable = true;
    this.editable = false;
    this.actionurl = s;
    this.qa$ = new BehaviorSubject<qa>({
      page: 1, pagesize: 10, queymodel: {},
      sort: {}
    });
    this.qa$.pipe(skip(1)).subscribe(async qa => {
      const mq = { ...qa.queymodel, sort: qa.sort };
      if (!this.qing) {
        console.log(qa);
        await this.query(mq, qa.pagesize, qa.page).toPromise();

      }
      //.toPromise();

      //}

    });
    if (this.pagerObj.Results$) {
      this.pagerObj.Results$.next(null);
    }
    this.targetQa$ = this.qa$;
    console.log("initTab "+s);
    //throw new Error("Method not implemented.");
  } 
  addable: boolean
  editable: boolean
  activatedTabIndex = 0;
  editobj$ = new BehaviorSubject<any>(null);
  sharehttp: HttpClient;
  sharebaseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private authorizeService: AuthorizeService,public alts: AlertService) {
    console.log('TablayoutService is constructed');
    this.sharehttp = http;
    this.sharebaseUrl = baseUrl;
    //this.ho.isPendingRequests.subscribe(o => {
    //  this.qing = o;
    //  //console.log(o);
    //});
     
  }
  genResful<T,X>(s: string,b=false) {
    return new Resful<T,X>(this, this.authorizeService, this.alts, s,b);
  }


  async ask<T>(k,mypath) {
    return await this.http.get<T>(`${this.baseUrl}api/${mypath}/${k}`).toPromise();
  }
  ngOnDestroy() {
    console.log('localService is destroyed');
  }


  prequery(q) {
    this.qa$.next({ ...this.qa$.getValue(), queymodel: q, page:1});
  }
  preAddquery(q) {
    this.qAdd$.next({ ...this.qAdd$.getValue(), queymodel: q, page: 1 });
  }

  fixnum(e: KeyboardEvent) {
    return (e.charCode === 8 || e.charCode === 0) ? null : e.charCode >= 48 && e.charCode <= 57
  }


  async add(o) {
    o.CreateDate = new Date();
    o.CreateId = await this.authorizeService.getUserIdFromStorage().toPromise();
    //console.log(o.CreateId);
    this.http.post(`${this.baseUrl}api/${this.actionurl}`, o).subscribe(_ => {
      this.qa$.next({
        ...this.qa$.getValue()
      });
      this.onSelectTab(null, 0);
      //this.activatedTabIndex = 0;
    }, err => {
      if (err.status === 500)
        console.log(err);
      this.alts.add(err.error, "danger");
    });
  }

  async mod(o, s) {
    o.UpdateDate = new Date();
    o.UpdateId = await this.authorizeService.getUserIdFromStorage().toPromise();
    console.log(o.UpdateId);

    this.http.put(`${this.baseUrl}api/${this.actionurl}/${s}`, o).subscribe(_ => {

      this.qa$.next({
        ...this.qa$.getValue()
      });
      //this.activatedTabIndex = 0;
      this.onSelectTab(null, 0);
      [this.editable, this.addable] = [this.addable, this.editable];
    }, err => {
        if (err.status === 500)
          console.log(err);
        this.alts.add(err.error, "danger");
    });
  }
  async del(o) {
    return await this.http.delete<any>(`${this.baseUrl}api/${this.actionurl}/${o}`).toPromise();
  }
  async getuid() {
    return await this.authorizeService.getUserIdFromStorage().toPromise();
  }
  async execProcess(o: any) {
    
    o.ProcessDate = new Date();
    o.ProcessId = await this.authorizeService.getUserIdFromStorage().toPromise();
    let rs = await this.http.post<IsResult>(`${this.baseUrl}api/Query/StockOrderProcess`, o).toPromise();
    this.alts.add(rs.Message, rs.Success ? "success" : "warning");
    
    this.syncq$.next({});
 
    return rs.Success;
  }

  
  onSelectTab(a, b) {
    //console.log(a);
    this.activatedTabIndex = b;
    this.targetQa$ = this.qa$;

    if (this.activatedTabIndex === 2) {
      this.editable = true;
      this.addable = false;
      this.targetQa$ = this.qAdd$;
    } else {
      this.editable = false;
      this.addable = true;
      if (this.activatedTabIndex === 1) {
        this.preAdd$.next({});
        this.targetQa$ = this.qAdd$;
      }
    }
  }

  chpage(p) {
    console.log(p);
    this.targetQa$ = this.activatedTabIndex === 0 ? this.qa$ : this.qAdd$;
    const qcurr = this.targetQa$.getValue();
    if (qcurr.page !== p) {
      this.targetQa$.next({ ...qcurr, page: p });
    }
  }
  chpagesize(s) {
    this.targetQa$.next({ ...this.targetQa$.getValue(), pagesize:s });
  }
  chsort(o) {
    this.targetQa$.next({ ...this.targetQa$.getValue(), sort: o });
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

   query<T>(q, s, p) {
    return this.http.post<IPagerRs<T>>(`${this.baseUrl}api/Query/${this.actionurl}/${s}/${p}`, q).pipe(take(1), catchError(err => {
      throw (err);
    }), map(o => {
      this.pagerObj = { ... this.pagerObj, ...o };

      if (this.pagerObj.Results$ == null) {
        this.pagerObj.Results$ = new BehaviorSubject<T[]>(o.Results);
      } else {

        this.pagerObj.Results$.next(o.Results);
      }
      return this.pagerObj;
    }));
    
  }
  queryAdd<T>(q, s, p) {
    return this.http.post<IPagerRs<T>>(`${this.baseUrl}api/Query/${this.actionqAddurl}/${s}/${p}`, q).pipe(take(1), catchError(err => {
      throw (err);
    }), map(o => {
      this.pagerAddObj = { ... this.pagerAddObj, ...o };

      if (this.pagerAddObj.Results$ === null) {
        this.pagerAddObj.Results$ = new BehaviorSubject<T[]>(o.Results);
      } else {
        console.log(o);
        this.pagerAddObj.Results$.next(o.Results);
      }
      return this.pagerAddObj;
    }));

  }
}
class qa {
  queymodel: any;
  page: number;
  pagesize: number;
  sort: any;

}
