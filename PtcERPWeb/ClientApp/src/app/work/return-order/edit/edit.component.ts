import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { Subject, BehaviorSubject, Observable } from 'rxjs';
import { SconfService } from '../../../shared/sconf.service';
import { takeUntil, take, filter, map } from 'rxjs/operators';
import { Resful } from '../../../Services/resful';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit, OnDestroy{
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  cud: Resful<any, any>;
  cudReturn: Resful<any, any>;
  refobj: any = {
    Customer: {}
  };
  preView = false;

  env = { nobar: false, BarF: '0', Barcode: "", doProcess: true, luckedit: false, preViewAll: false,};
  private ngUnsubscribe = new Subject();
  locbarcodes$ = new BehaviorSubject<IRReturnOrderAdd[]>([]);
  fmax$ = new BehaviorSubject<number>(0);

  locRs$: Observable<IRReturnOrderAdd[]>;
  locMapRs$ = new BehaviorSubject<IRReturnOrderAdd[]>([]);
  nors: boolean;



  constructor(public tl: TablayoutService, public sconf: SconfService) {
    this.cud = this.tl.genResful('ReturnOrders');
    this.cudReturn = this.tl.genResful('ReturnOrderEditQuery');
    tl.editobj$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(async o => {
      console.log(o);
      
      this.refobj = { ...o, CustomerId: o?.Customer?.CustomerInfoId };
      
      this.cudReturn.preQuery({ ...this.refobj });

 
      //await this.stockOrderchange(this.refobj.StockOrderId);

    });
    this.locRs$ = this.cudReturn.pagerObj.Results$.pipe(takeUntil(this.ngUnsubscribe), filter(t => t != null), map((t: IRReturnOrderAdd[]) => {

      const maprs = this.locMapRs$.getValue();
      t.forEach((o, i) => {
        const d = maprs.find(r => r.PickOrderSubId == o.PickOrderSubId);
        if (d) {
          o.Returned = d.Returned
          o.ReturnPrice = d.ReturnPrice;
          o.ReturnReason = d.ReturnReason;

          //console.log(d);
        }
        o.chreturned= () => {
          let xmaprs = this.locMapRs$.getValue();
          if (!o.FReturned) {
            if (o.Returned) {
              if (this.refobj.MainResson) {
                if (!o.ReturnReason) {
                  o.ReturnReason = this.refobj.MainResson;
                }
              }
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


  }

  //async stockOrderchange(d: string) {
  //  if (d.length > 0) {
  //    this.locbarcodes$.next([]);
  //    this.locMapRs$.next([]);
  //  }

  //  let o = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/StockOrderBarcodes/' + d).pipe(take(1)).toPromise();
  //  this.locbarcodes$.next(o);
  //  const localmaprs = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/StockOrderEditMap/' + d).pipe(take(1)).toPromise();
  //  this.locMapRs$.next(localmaprs);

  //  this.tl.preAddquery({ StockOrderId: d, OrgStockUnitId: this.refobj.OrgStockUnitId, ProcessDate: this.refobj.ProcessDate });

  //}

  
  


  //#region   typeahead
  onSelect(e) {
    if (!this.nors && !this.preView) {

      const maprs = this.locMapRs$.getValue();
      const d = maprs.find(o => o.BarcodeId == e.item.BarcodeId);
      if (!d) {
        // maprs = [...maprs, e.item];
        this.locMapRs$.next([...maprs, { ...e.item, Selected: true }]);
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
      }

    }
  }




  //#endregion



  async doMod() {
    
    await this.cud.mod(this.refobj, this.refobj.ReturnOrderId);
    this.tl.syncq$.next({});
 
     

    //this.tl.mod(o, o.StockOrderId);



  }

  ngOnInit() {
  }

}
