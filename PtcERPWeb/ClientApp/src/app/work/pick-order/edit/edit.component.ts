import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { Subject, BehaviorSubject } from 'rxjs';
import { SconfService } from '../../../shared/sconf.service';
import { takeUntil, take, filter } from 'rxjs/operators';
import { Resful } from '../../../Services/resful';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit, OnDestroy{
  xl: Resful<any,any>;
  cud: Resful<any,any>;
  ngOnDestroy(): void {

    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  refobj: any = {};
  locstockunits$ = new BehaviorSubject<IIdName[]>(null);
  preView = false;

  env = { nobar: false, BarF: '0', Barcode: "", doProcess: true ,luckedit:false};
  private ngUnsubscribe = new Subject();
  locbarcodes$ = new BehaviorSubject<IIdCount[]>([]);
  fmax$ = new BehaviorSubject<number>(0);
  locRs$ = new BehaviorSubject<IIdCount[]>([]);
  locMapRs$ = new BehaviorSubject<IIdCount[]>([]);

  nors: boolean;



  constructor(public tl: TablayoutService, public sconf: SconfService) {
    this.cud = this.tl.genResful('PickOrders');
    this.xl = this.tl.genResful('PickOrderEditQuery');
    tl.editobj$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(async o => {
      

      this.refobj = { ...o };
      this.env.luckedit = this.refobj.ProcessDate  ? true:false;
      this.locstockunits$.next(sconf.xstockunits$.getValue().filter(u => u.Id !== this.refobj.OrgStockUnitId));
      await this.Orderchange(this.refobj.PickOrderId);

    });
    this.xl.pagerObj.Results$.pipe(takeUntil(this.ngUnsubscribe), filter(t => t !== null)).subscribe(t => {
      const maprs = this.locMapRs$.getValue();
      t.forEach(o => {
        const d = maprs.find(x => x.WithBarcodes ? x.BarcodeId === o.BarcodeId : x.FormateId === o.FormateId);
        if (d) {
          if (d.WithBarcodes) {
            o.Selected = true;
          } else {
            if (!o.BarcodeId)
              o.SetCount = d.SetCount;
          }
        }
      });
      this.locRs$.next(t);
    });

    this.locMapRs$.subscribe(maprs => {
      console.log(maprs);
      const t = this.locRs$.getValue();
      t.forEach(o => {
        const d = maprs.find(x => o.WithBarcodes ? x.BarcodeId === o.BarcodeId : x.FormateId === o.FormateId);
        if (d) {
          o.Selected = d.Selected;
          if (!o.BarcodeId) {
            o.SetCount = d.SetCount;
          }
        }
      });
      this.locRs$.next(t);
    });


  }

  async Orderchange(d: string) {
    if (d.length > 0) {
      this.locbarcodes$.next([]);
      this.locMapRs$.next([]);
    }
    const o = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/PickOrderBarcodes/' + d).pipe(take(1)).toPromise();
    this.locbarcodes$.next(o);
    const localmaprs = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/PickOrderEditMap/' + d).pipe(take(1)).toPromise();
    this.locMapRs$.next(localmaprs);

    this.xl.preQuery({ PickOrderId: d, StockUnitId: this.refobj.StockUnitId, ProcessDate: this.refobj.ProcessDate });
  }

  async dvchange(d) {
    if (d) {
      if (this.refobj.OrgStockUnitId == this.refobj.StockUnitId) {
        this.refobj.StockUnitId = null;
      }
      if (this.refobj.StockUnitId && this.refobj.StockUnitId > 0) {
        //const orgs = this.sconf.xstockunits$.getValue();
        //this.locstockunits$.next(orgs.filter(o => o.Id != this.refobj.StockUnitId));
        this.refobj.Keeper = (await this.tl.sharehttp.get<{ Keeper: string }>(`${this.tl.sharebaseUrl}api/StockUnits/${this.refobj.StockUnitId}`).toPromise()).Keeper;
      }
    }
  }




  fixPost() {
    this.xl.pagerObj.CurrentPage = 1;
    if (this.preView) {
      const lm = this.locMapRs$.getValue();
      const BarcodeIds = lm.filter(o => o.BarcodeId).map(o => o.BarcodeId);
      const FormateNumbrs = lm.filter(o => !o.WithBarcodes).map(x => ({ FormateId: x.FormateId, SetCount: x.SetCount }));
      this.xl.preQuery({
        PickOrderId: this.refobj.PickOrderId,
        StockUnitId: this.refobj.StockUnitId, ProcessDate: this.refobj.ProcessDate ,
        AskPre: true,
        BarcodeIds: BarcodeIds,
        FormateNumbrs: FormateNumbrs
      });
    } else {

      this.xl.preQuery({
        PickOrderId: this.refobj.PickOrderId,
        StockUnitId: this.refobj.StockUnitId, ProcessDate: this.refobj.ProcessDate 
      });
    }
  }
  fitmap(e: IIdCount) {
    let maprs = this.locMapRs$.getValue();
    console.log(e);
    if (e.WithBarcodes) {
      if (e.Selected) {
        maprs = [...maprs, e];
      } else {
        maprs = this.shiftmap(maprs, e.BarcodeId, e.FormateId)
      }
    } else {
      if (e.SetCount > 0) {
        if (e.SetCount > e.Count) {
          e.SetCount = e.Count;
        }
        const d = maprs.find(o => o.FormateId === e.FormateId);
        if (d) {
          d.SetCount = e.SetCount;
        } else {
          console.log(e);
          maprs = [...maprs, { ...e }];
        }
      } else {
        e.SetCount = 0;
        maprs = this.shiftmap(maprs, e.BarcodeId, e.FormateId)
      }
    }
    this.locMapRs$.next(maprs);
  }
  shiftmap(maprs: IIdCount[], barcodeId: string, formateId: number) {
    if (barcodeId) {
      maprs = maprs.filter(o => o.BarcodeId !== barcodeId);
      console.log(maprs);
    } else {
      if (formateId) {
        maprs = maprs.filter(o => o.FormateId !== formateId);
      }
    }
    return maprs;
  }
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


        this.locMapRs$.next([...maprs, { ...bb, Selected: true }]);
      }

    }
  }




  //#endregion
  async doMod() {
    const lm = this.locMapRs$.getValue();
    const FormateNumbrs = lm.filter(o => !o.WithBarcodes).map(x => ({ FormateId: x.FormateId, SetCount: x.SetCount }));
    let BarcodeIds = lm.filter(o => o.BarcodeId).map(o=>o.BarcodeId);
   // const fbs = await this.tl.sharehttp.post<IStockOrderSub[]>(`${this.tl.sharebaseUrl}api/Query/AskStockOrderFormateBarcodes`, { StockOrderId: this.refobj.StockOrderId, FormateNumbrs: FormateNumbrs, BarcodeIds: BarcodeIds }).toPromise();
    const fbs = await this.tl.sharehttp.post<string[]>(`${this.tl.sharebaseUrl}api/Query/AskPickOrderFormateBarcodes`, { PickOrderId: this.refobj.PickOrderId, StockUnitId: this.refobj.StockUnitId, FormateNumbrs: FormateNumbrs }).toPromise();
    BarcodeIds = BarcodeIds.concat(fbs);

    const o = { ...this.refobj, PickOrderSubs: BarcodeIds.map(p => ({ BarcodeId: p })) };
   


    console.log(o);
    const j = await this.cud.del(o.PickOrderId);
    if (j) {
      delete o.PickOrderId;
      await this.cud.add(o);
    }

    //this.tl.mod(o, o.StockOrderId);



  }

  ngOnInit() {
  }

}
