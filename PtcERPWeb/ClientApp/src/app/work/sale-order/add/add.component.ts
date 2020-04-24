import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { BehaviorSubject, Observable, Subscription, Subject } from 'rxjs';
import { take, map, filter, takeUntil, skip } from 'rxjs/operators';
import { AlertService } from '../../../shared/alert.service';
import { Router } from '@angular/router';
import { PageChangedEvent } from 'ngx-bootstrap/pagination/ngx-bootstrap-pagination';
import { IPagerRs } from '../../../Models/Ipager-rs';
import { Resful } from '../../../Services/resful';
import { isNumber } from 'util';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit,OnDestroy {
    cudSaleLoc$: Observable<any[]>;
  ngOnDestroy(): void {
   
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  refobj: any = {
  Customer: {}};
  preView = false;
  pickobj:any

  env = {
    preViewAll: false,
    nobar: false, BarF: '0', Barcode: "", doProcess: true, OrgStockUnitId: 0, OrgStockUnitName: null, locp: { page: 1, itemsPerPage: 10 }
  };
  //xstockunits$ =
  locstockunits$ = new BehaviorSubject<IIdName[]>(null);
  locformate$ = new BehaviorSubject<IIdCount[]>(null);
  locbarcodes$ = new BehaviorSubject<IIdCount[]> ([]);
  fmax$ = new BehaviorSubject<number>(0);

  locRs$ = new BehaviorSubject<IPick[]>([]);
 


  locMapRs$ = new BehaviorSubject<IPick[]>([]);
  locSaleMapRs$ = new BehaviorSubject<any[]>([]);
  locSaleMapRs: IIdCount[];

  asyncSelected: string;
  typeaheadLoading: boolean;
 
  dataSource: Observable<any>;
    nors: boolean;
  cud: Resful<any,any>;
  xl: Resful<any,any>;
  cudSale: Resful<any,any>;
  private ngUnsubscribe = new Subject();
  constructor(public tl: TablayoutService, public sconf: SconfService, private alts: AlertService, router: Router) {
    this.cud = this.tl.genResful('PickOrders');
    this.cudSale = this.tl.genResful('SaleOrdersDetail');



    this.xl = this.tl.genResful('PickOrderEditQuery');
    tl.preAdd$.pipe(skip(1),takeUntil(this.ngUnsubscribe)).subscribe(async o => {
      const allprocess = await this.tl.sharehttp.get<boolean>(this.tl.sharebaseUrl + 'api/Query/AllStockOrderProsess/').pipe(take(1)).toPromise();
      if (!allprocess) {
        console.log(allprocess);

        tl.onSelectTab(null, 0);
        await router.navigate(['/work/stock-order']);


        alts.add("有未執行的轉倉單須先執行", "danger");


      } else {


    


        o.Customer = {};
        this.refobj = o;

        this.preView = false;
        this.env =  {
          preViewAll: false,
          nobar: false, BarF: '0', Barcode: "", doProcess: true, OrgStockUnitId: 0, OrgStockUnitName: null, locp: { page: 1, itemsPerPage: 10 }
        }
        this.refobj.Magnification = 1;
        this.cud.pagerObj.Results$.next(null);
        this.xl.pagerObj.Results$.next(null);
        this.locMapRs$.next([]);
        this.locstockunits$.next(sconf.xstockunits$.getValue());
      }
    });
    this.cud.pagerObj.Results$.pipe(takeUntil(this.ngUnsubscribe), filter(t => t != null)).subscribe(t => {
      const maprs = this.locMapRs$.getValue();
      if (t.length === 0) {
        this.alts.add("無可用檢貨單", "danger");
      }
      t.forEach(o => {
        o.read = async () => {
          if (this.preView) {
            const lm = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/PickOrderEditMap/' + o.PickOrderId).pipe(take(1)).toPromise();
            const BarcodeIds = lm.filter(o => o.BarcodeId).map(o => o.BarcodeId);
            const FormateNumbrs = lm.filter(o => !o.WithBarcodes).map(x => ({ FormateId: x.FormateId, SetCount: x.SetCount }));
            this.xl.preQuery({
              PickOrderId: o.PickOrderId,
              StockUnitId: o.StockUnitId, ProcessDate: new Date(),
              AskPre: true,
              BarcodeIds: BarcodeIds,
              FormateNumbrs: FormateNumbrs
            });
          } else {
            this.xl.preQuery({
              AskPre: false,
              PickOrderId: o.PickOrderId,
              StockUnitId: o.StockUnitId, ProcessDate:new Date() //o.ProcessDate
            });
          }
        }
        let d = maprs.find(x => x.PickOrderId === o.PickOrderId);
        if (d) {
          o.Selected = true;
        }
      });
      this.locRs$.next(t);
    });



    this.locMapRs$.subscribe(maprs => {
      console.log(maprs);
      let t = this.locRs$.getValue();
      t.forEach(o => {
        let d = maprs.find(x => x.PickOrderId == o.PickOrderId);
        if (d) {
          o.Selected = d.Selected;
        }
      });
      this.locRs$.next(t);
    });


    this.locSaleMapRs$.subscribe(rs => {
      this.refobj.OrderPrice = rs.length >0 ?  rs.map(o => o.SalePrice).reduce((a, b) => a + b): 0;

    });
    this.cudSaleLoc$= this.cudSale.pagerObj.Results$.pipe(takeUntil(this.ngUnsubscribe), filter(t => t != null), map(t => {
      const ls = this.locSaleMapRs$.getValue();
      t.forEach(x => {
        x.SalePrice = ls.find(o => o.PickOrderSubId == x.PickOrderSubId).SalePrice;

        x.chPrice = (xPrice) => {
          if (isNumber(xPrice)) {
            const lls = this.locSaleMapRs$.getValue();
            lls.find(o => o.PickOrderSubId == x.PickOrderSubId).SalePrice = xPrice;
            this.locSaleMapRs$.next(lls);
          }

        }
      });
      console.log(t);
      return t;})
    );
    


  


  }
  async dvchange(d) {
    if (d) {
      
      if (this.refobj.CustomerId > 0) {
        this.refobj.Customer = await this.tl.sharehttp.get<any>(`${this.tl.sharebaseUrl}api/CustomerInfos/${this.refobj.CustomerId}`).toPromise();
        console.log(this.refobj);
        
        this.cud.preQuery({ ForSqle:true});

      }
    }
  }


   
 
  async genPost() {
    //this.tl.pagerAddObj.CurrentPage = 1;
    if (this.env.preViewAll && isNumber(this.refobj.Magnification) && this.refobj.Magnification > 0 ) {
      const lm = this.locMapRs$.getValue();
      const q = {
        Magnification: this.refobj.Magnification,
        PickOrderIds: lm.map(p => p.PickOrderId)
      }
      const rs =await this.tl.sharehttp.post< any[]>(`${this.tl.sharebaseUrl}api/Query/SaleOrderMap`, q).pipe(take(1)).toPromise();
      this.locSaleMapRs$.next(rs);
      this.alts.add("確認售價後新增", "success");
      this.cudSale.preQuery(q);


      //this.tl.preAddquery({
      //  OrgStockUnitId: this.refobj.OrgStockUnitId,
      //  AskPre: true,
      //  BarcodeIds: BarcodeIds,
      //  FormateNumbrs: FormateNumbrs
      //});
    } else {
       
      //this.tl.preAddquery({
      //  OrgStockUnitId: this.refobj.OrgStockUnitId,
      //});
    }
  }
  async doAdd() {
    const lm = this.locMapRs$.getValue();
    //const FormateNumbrs = lm.filter(o => !o.WithBarcodes).map(x => ({ FormateId: x.FormateId, SetCount: x.SetCount }));
    //let BarcodeIds = lm.filter(o => o.BarcodeId).map(o => o.BarcodeId);
    //const fbs = await this.tl.sharehttp.post<string[]>(`${this.tl.sharebaseUrl}api/Query/AskFormateBarcodes`, { OrgStockUnitId: this.refobj.OrgStockUnitId, FormateNumbrs: FormateNumbrs }).toPromise()
    //BarcodeIds = BarcodeIds.concat(fbs);

    const q = this.locSaleMapRs$.getValue();
    const fixOrderPrices = await this.tl.sharehttp.post<boolean>(`${this.tl.sharebaseUrl}api/Query/FixOrderPrices`, q).pipe(take(1)).toPromise();
    if (fixOrderPrices) {
      let o = { ...this.refobj, PickOrders: lm.map(p => ({ PickOrderId: p.PickOrderId })) }
      console.log(o);




      await this.tl.genResful('SaleOrders').add(o);
    }






  }
  //AskFormateBarcodes

  resetStockForm() {
    this.locbarcodes$.next([]);
    this.locMapRs$.next([]);
    this.env.OrgStockUnitId = 0;
    this.env.Barcode = "";
    this.tl.pagerAddObj.Results$.next(null);
  }
  //gneSaleLoc() {
  //  const maprs = this.locMapRs$.getValue();
  //  const mapSrs = this.locSaleMapRs$.getValue();


  //  maprs.forEach(p => {
  //    if (p.WithBarcodes) {
  //      if (mapSrs.some(o => o.BarcodeId == p.BarcodeId)) {
  //        this.alts.add(`Barcode: ${p.BarcodeId} 重複勾選了`, 'info');
  //      } 
  //    } else   {
  //      if (mapSrs.some(o => o.FormateId == p.FormateId && o.StockUnitId == this.env.OrgStockUnitId)) {
  //        this.alts.add(`Formate: ${p.FormateId} 重複了,將賦予新值`, 'info');
  //      } 
  //    }
  //  });
  //  let [bs, fs] = [mapSrs.filter(o => o.WithBarcodes), mapSrs.filter(o => !o.WithBarcodes)];
  //  const bids=bs.map(o => o.BarcodeId);
  //  const bfmaprs = maprs.filter(o => o.WithBarcodes && !bids.includes(o.BarcodeId)).map(o => ({ ...o, StockUnitId: this.env.OrgStockUnitId, StockUnitName: this.env.OrgStockUnitName } as IIdCount));
  //  bs = bs.concat(bfmaprs);
  //  const ffmaprs = maprs.filter(o => !o.WithBarcodes).map(o => ({ ...o, StockUnitId: this.env.OrgStockUnitId, StockUnitName: this.env.OrgStockUnitName } as IIdCount));
  //  fs = fs.map(f => ffmaprs.find(o => o.FormateId === f.FormateId && o.StockUnitId == f.StockUnitId) || f);//重複了,將賦予新值
  //  for (const f of ffmaprs) {
  //    if (!fs.some(o => o.FormateId == f.FormateId && o.StockUnitId == f.StockUnitId)) {
  //      fs=[...fs,f];
  //    }
  //  }
  //  this.locSaleMapRs$.next([...bs ,...fs]);
  //}


  pageChanged(event: PageChangedEvent): void {
    this.env.locp = { ...event };
    const startItem = (event.page - 1) * event.itemsPerPage;
    const endItem = event.page * event.itemsPerPage;
    this.locSaleMapRs = this.locSaleMapRs$.getValue().slice(startItem, endItem);
  }
  locdel(e: IIdCount) {

    const f = this.locSaleMapRs$.getValue().filter(o => e.WithBarcodes ? o.BarcodeId != e.BarcodeId :
      o.WithBarcodes ? true :
        !(o.FormateId == e.FormateId && o.StockUnitId == e.StockUnitId));
    this.locSaleMapRs$.next(f);

  }


  async subdvchange(d) {
    this.env.OrgStockUnitName = null;
    if (d > 0) {
      this.locbarcodes$.next([]);
      this.locMapRs$.next([]);
      this.env.OrgStockUnitName = this.sconf.xstockunits$.getValue().find(o => o.Id == d).Name;
    }
    const o = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/StockUnitBarcodes/' + d).pipe(take(1)).toPromise();
    this.locbarcodes$.next(o);
    this.tl.preAddquery({ OrgStockUnitId: d });
  }

  fitmap(e: IPick) {
    let maprs = this.locMapRs$.getValue();
    console.log(e);

    if (e.Selected) {
      maprs = [...maprs, e];
    } else {
      maprs = this.shiftmap(maprs, e.PickOrderId)
    }

    this.locMapRs$.next(maprs);
  }
  shiftmap( maprs: IPick[], pickorderId: string ) {
     
    maprs = maprs.filter(o => o.PickOrderId !== pickorderId);
    return maprs;
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
