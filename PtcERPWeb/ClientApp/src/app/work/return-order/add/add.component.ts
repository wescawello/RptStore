import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { BehaviorSubject, Observable, Subscription, Subject } from 'rxjs';
import { take, map, filter, takeUntil, skip } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { AlertService } from '../../../shared/alert.service';
import { Router } from '@angular/router';
import { PageChangedEvent } from 'ngx-bootstrap/pagination/ngx-bootstrap-pagination';
import { IPagerRs } from '../../../Models/Ipager-rs';
import { Resful } from '../../../Services/resful';


@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit, OnDestroy {
  cud: Resful<any, any>;
  cudReturn: Resful<any, any>;
  AllReturnPrice: number;
  ngOnDestroy(): void {
     
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  refobj: any = {
    Customer: {}
  };
  preView = false;

  env = {
    ViewFormate: false, preViewAll:false,
    nobar: false, BarF: '0', Barcode: "", doProcess: true, OrgStockUnitId: 0, OrgStockUnitName: null, locp: { page: 1, itemsPerPage: 10 }
  };
  //xstockunits$ =
  locstockunits$ = new BehaviorSubject<IIdName[]>(null);
  locformate$ = new BehaviorSubject<IIdCount[]>(null);
  locbarcodes$ = new BehaviorSubject<IRReturnOrderAdd[]>([]);
  fmax$ = new BehaviorSubject<number>(0);

  locRs$: Observable<IRReturnOrderAdd[]>;



  locMapRs$ = new BehaviorSubject<IRReturnOrderAdd[]>([]);
  locSaleMapRs: IIdCount[];
  asyncSelected: string;
  typeaheadLoading: boolean;

  nors: boolean;

  private ngUnsubscribe = new Subject();
  constructor(public tl: TablayoutService, public sconf: SconfService, private alts: AlertService, router: Router) {
    this.cud = this.tl.genResful('ReturnOrders');
    this.cudReturn = this.tl.genResful('ReturnOrderAddQuery');

    tl.preAdd$.pipe(skip(1), takeUntil(this.ngUnsubscribe)).subscribe(async o => {
      o.Customer = {};
      this.refobj = o;
      this.preView = false;
      this.cudReturn.pagerObj.Results$.next(null);
      this.locMapRs$.next([]);
      this.locstockunits$.next(sconf.xstockunits$.getValue());
    });
    this.locRs$ = this.cudReturn.pagerObj.Results$.pipe(takeUntil(this.ngUnsubscribe), filter(t => t != null), map((t: IRReturnOrderAdd[]) => {

      let maprs = this.locMapRs$.getValue();
      t.forEach((o, i) => {
        const d = maprs.find(r => r.PickOrderSubId == o.PickOrderSubId);
        if (d) {
          o.Returned = d.Returned
          o.ReturnPrice = d.ReturnPrice;
          o.ReturnReason = d.ReturnReason;

          //console.log(d);
        }
        o.chreturned = () => {
          let xmaprs = this.locMapRs$.getValue();
          if (!o.FReturned) {
            if (o.Returned) {
              if (this.refobj.MainResson) {
                if (!o.ReturnReason) {
                  o.ReturnReason = this.refobj.MainResson;
                }
              }
              o.ReturnPrice = o.SalePrice;
              const f = xmaprs.find(r => r.PickOrderSubId == o.PickOrderSubId);
              if (!f) {
                xmaprs = [...xmaprs, o];
              }
            } else {
              xmaprs = xmaprs.filter(x => o.PickOrderSubId !== x.PickOrderSubId);
            }
            this.locMapRs$.next(xmaprs);
            //console.log(maprs);
          }
        }
      });
      return t;
    }));
    this.locMapRs$.subscribe(rs => {
      this.AllReturnPrice = rs.length > 0 ? rs.map(o => o.ReturnPrice).reduce((a, b) => a + b) : 0;
    });
  }
  async dvchange(d) {
    if (d) {

      if (this.refobj.CustomerId > 0) {
        this.refobj.Customer = await this.tl.sharehttp.get<any>(`${this.tl.sharebaseUrl}api/CustomerInfos/${this.refobj.CustomerId}`).toPromise();
        console.log(this.refobj);
        const o = await this.tl.sharehttp.get<IRReturnOrderAdd[]>(this.tl.sharebaseUrl + 'api/Query/ReturnOrderAddQueryNopage/' + d).pipe(take(1)).toPromise();
        this.locbarcodes$.next(o);

        this.cudReturn.preQuery({ ...this.refobj, PickOrderSubIds: [] });
      }
    }
  }


  //#region   typeahead
  onSelect(e) {
    if (!this.nors && !this.preView) {
      const maprs = this.locMapRs$.getValue();
      const d = maprs.find(o => o.BarcodeId == e.item.BarcodeId);
      if (!d) {
        this.locMapRs$.next([...maprs, { ...e.item, ReturnReason: this.refobj.MainResson, ReturnPrice: e.item.ReturnPrice,Returned: true }]);
        this.cudReturn.pagerObj.Results$.next(this.cudReturn.pagerObj.Results$.getValue());
      }
      console.log(e);
    }
  }
  typeaheadNoResults(e: boolean) {
    this.nors = e;
  }
  typeaheadOnBlur(e) {
    if (!this.nors) {
      const maprs = this.locMapRs$.getValue();
      const d = maprs.find(o => o.BarcodeId == e.item.BarcodeId);
      if (!d) {
        // maprs = [...maprs, e.item];
        //this.locMapRs$.next([...maprs, {... e.item,Selected:true }]);
      }
      this.env.Barcode = e.item.BarcodeValue
      console.log(e.item);
    }
  }
  typeaheadOnBtn(s: string) {
    if (!this.nors) {
      const maprs = this.locMapRs$.getValue();
      const d = maprs.find(o => o.BarcodeValue == s);
      if (!d) {
        const bb = this.locbarcodes$.getValue().find(o => o.BarcodeValue == s)
        this.locMapRs$.next([...maprs, { ...bb, Returned: true }]);
        this.cudReturn.pagerObj.Results$.next(this.cudReturn.pagerObj.Results$.getValue());
      }

    }
  }




  //#endregion

  genPost() {
    if (this.preView) {
      this.cudReturn.preQuery({ ...this.refobj, PickOrderSubIds: this.locMapRs$.getValue().map(x => x.PickOrderSubId) });
    } else {
      this.cudReturn.preQuery({ ...this.refobj, PickOrderSubIds: [] });
    }
  }
  async doAdd() {
    const returnOrderSubs = this.locMapRs$.getValue().map(x => {
      return { PickOrderSubId: x.PickOrderSubId, ReturnPrice: x.ReturnPrice, ReturnReason: x.ReturnReason };
    });
    await this.cud.add({ ...this.refobj, ReturnOrderSubs: returnOrderSubs });
  }
  //AskFormateBarcodes

  resetStockForm() {
    this.locbarcodes$.next([]);
    this.locMapRs$.next([]);
    this.env.OrgStockUnitId = 0;
    this.env.Barcode = "";
    this.cudReturn.pagerObj.Results$.next(null);
  }


   

   




  async BarFchange() {
    if (this.env.BarF == '0') {
      // this.locbarcodes$.next(this.locformate$.getValue()[0].Barcodes);


    } else {
      //this.fmax$.next(this.locformate$.getValue().find(o => o.Id == this.env.BarF).Count);

    }

  }
  ngOnInit() {

  }
  // fired every time search string is changed

}
