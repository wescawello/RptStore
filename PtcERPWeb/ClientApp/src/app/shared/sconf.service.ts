import { Injectable, Inject } from '@angular/core';
import { Observable, BehaviorSubject, forkJoin } from 'rxjs';
import { filter, mapTo, expand, take, map, switchMap, mergeMap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { isArray } from 'util';


@Injectable({
  providedIn: 'root'
})
export class SconfService {
  vandors$ = new BehaviorSubject<IIdName[]>(null);

  mvandor$ = new BehaviorSubject<IIdName[]>(null);
  xvandors$ = new BehaviorSubject<IIdName[]>(null);
  xdevices$ = new BehaviorSubject<IIdName[]>(null);
  edmvandor$ = new BehaviorSubject<any[]>(null);

  xstockunits$ = new BehaviorSubject<IIdName[]>(null);
  xstockunitsAdmin$ = new BehaviorSubject<IIdName[]>(null);
  xcustomers$ = new BehaviorSubject<IIdName[]>(null);

  product$ = new BehaviorSubject<(IIdName & {tag:boolean})[]>(null);
  constructor(private authorize: AuthorizeService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

    authorize.isAuthenticated().pipe(filter(o => { console.log(o);; return o == true; }), take(1), mergeMap(_ => {
      const vandors =http.get<IIdName[]>(`${this.baseUrl}api/Query/ForDropDown?dlType=Vendors`).pipe(take(1));
      const mvandor = http.get<IIdName[]>(`${this.baseUrl}api/Query/ForDropDown?dlType=MVendors`).pipe(take(1));
      const xvandors = http.get<IIdName[]>(`${this.baseUrl}api/Query/ForVD/0/vandors`).pipe(take(1));
      const xdevices = http.get<IIdName[]>(`${this.baseUrl}api/Query/ForVD/0/devices`).pipe(take(1));
      const xstockunits = http.get<IIdName[]>(`${this.baseUrl}api/Query/ForVD/0/stockunits`).pipe(take(1));
      const xstockunitsAdmin = http.get<IIdName[]>(`${this.baseUrl}api/Query/ForVD/0/stockunitsAdmin`).pipe(take(1));

      const xcustomers = http.get<any[]>(`${this.baseUrl}api/CustomerInfos`).pipe(take(1), map(o => o.map(x => ({ Name: x.Name, Id: x.CustomerInfoId }) as IIdName) ));
      return forkJoin({ vandors, mvandor, xvandors, xdevices, xstockunits, xstockunitsAdmin, xcustomers })
    })).subscribe(o => {

      Object.keys(o).forEach(k => {
        console.log(k);
        (this[k + '$'] as BehaviorSubject<IIdName[]>).next(o[k]);
      });

      //this.vender$.next(o.vendors);
      //this.mvender$.next(o.mvendors);

 
      //this.xvandors$.next(o.xvandors);
      //this.xdevices$.next(o.xdevices);






      //this.product$.next(o.vandors.map(x => x.Items).reduce((a, b) => [...a, ...b]).map(z => ({ ...z, tag: true })));




    });



  }
  fixedmvender(o) {
    console.log(o);
    this.edmvandor$.next(o)
  }
  chvd(i, x) {
    if (isArray(i)) {
      return this.http.get<IIdName[]>(`${this.baseUrl}api/Query/ForVD/${i[0]}/${x}/${i[1]}`).pipe(take(1));
    } else {
      return this.http.get<IIdName[]>(`${this.baseUrl}api/Query/ForVD/${i}/${x}`).pipe(take(1));
    }
  }


  revtag(d, q) {
    console.log(q);
    if (d.tag) {
       
      q.vx = this.vandors$.getValue().filter(a => a.Items.filter(b => b.Id == d.Id).length > 0)[0];
      q.vy = q.vx.Items.find(b => b.Id == d.Id);
      console.log(q);
    }
  

  }
}
