import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { BehaviorSubject,    Subject } from 'rxjs';
import { take,  filter, takeUntil } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { AlertService } from '../../../shared/alert.service';
import { Resful } from '../../../Services/resful';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit,OnDestroy {
  xl: Resful<any, any>;
  cud: Resful<any,any>;
  ngOnDestroy(): void {
   
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  refobj: any = {};
  preView = false;
  env = { nobar: false, BarF: '0', Barcode: "", doProcess:true};
  //xstockunits$ =
  locstockunits$ = new BehaviorSubject<IIdName[]>(null);
  locformate$ = new BehaviorSubject<IIdCount[]>(null);
  locbarcodes$ = new BehaviorSubject<IIdCount[]> ([]);
  fmax$ = new BehaviorSubject<number>(0);
  allOrgPrice$ = new BehaviorSubject<number>(0);
  locRs$ = new BehaviorSubject<IIdCount[]>([]);
 


  locMapRs$ = new BehaviorSubject<IIdCount[]>([]);
  asyncSelected: string;
  typeaheadLoading: boolean;
 
  nors: boolean;

  private ngUnsubscribe = new Subject();
  constructor(public tl: TablayoutService, public sconf: SconfService, private  alts: AlertService) {
    this.cud = this.tl.genResful('PickOrders');
    this.xl = this.tl.genResful('PickOrderAddQuery');
    tl.preAdd$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(async o => {
      const allprocess = await this.tl.sharehttp.get<boolean>(this.tl.sharebaseUrl + 'api/Query/AllStockOrderProsess/').pipe(take(1)).toPromise();
      if (!allprocess) {
        console.log(allprocess);
        tl.onSelectTab(null, 0);
        alts.add("有未執行的轉倉單須先執行", "danger");
      } else {
        this.refobj = o;
        this.preView = false;
        this.xl.pagerObj.Results$.next(null);
        this.locMapRs$.next([]);
      }
    });
    this.xl.pagerObj.Results$.pipe(takeUntil(this.ngUnsubscribe), filter(t => t != null)).subscribe(t => {
      const maprs = this.locMapRs$.getValue();
      t.forEach(o => {
        const d = maprs.find(x => x.WithBarcodes ? x.BarcodeId == o.BarcodeId : x.FormateId == o.FormateId);
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
        const d = maprs.find(x => o.WithBarcodes ? x.BarcodeId == o.BarcodeId : x.FormateId == o.FormateId);
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
  async dvchange(d) {
    this.locbarcodes$.next([]);
    this.locMapRs$.next([]);
    this.locRs$.next([]);
    if (d) {
      this.refobj.Keeper = (await this.tl.sharehttp.get<{ Keeper: string }>(`${this.tl.sharebaseUrl}api/StockUnits/${this.refobj.StockUnitId}`).toPromise()).Keeper;
      const o = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/StockUnitBarcodes/' + d).pipe(take(1)).toPromise();
      this.locbarcodes$.next(o);
      this.xl.preQuery({ OrgStockUnitId: d });
    }  
  }



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
      this.env.Barcode= e.item.BarcodeValue
      console.log(e.item);
    }
  }
  typeaheadOnBtn(s: string) {
    if (!this.nors) {
      const maprs = this.locMapRs$.getValue();
      const d = maprs.find(o => o.BarcodeValue == s);
      if (!d) {
        const bb=this.locbarcodes$.getValue().find(o => o.BarcodeValue == s)

       
        this.locMapRs$.next([...maprs, { ...bb, Selected: true }]);
      }
      
    }
  }
  genPost() {
    this.xl.pagerObj.CurrentPage = 1;
    if (this.preView) {
      const lm = this.locMapRs$.getValue();
      const BarcodeIds = lm.filter(o => o.BarcodeId).map(o => o.BarcodeId);
      const FormateNumbrs = lm.filter(o => !o.WithBarcodes).map(x => ({ FormateId: x.FormateId, SetCount: x.SetCount }));
      this.xl.preQuery({
        OrgStockUnitId: this.refobj.OrgStockUnitId,
        AskPre: true,
        BarcodeIds: BarcodeIds,
        FormateNumbrs: FormateNumbrs
      });

 




    } else {
      this.xl.preQuery({
        OrgStockUnitId: this.refobj.OrgStockUnitId,
      });
      this.allOrgPrice$.next(0);
    }
  }
  async doAdd() {
    const lm = this.locMapRs$.getValue();
    const FormateNumbrs = lm.filter(o => !o.WithBarcodes).map(x => ({ FormateId: x.FormateId, SetCount: x.SetCount }));
    let BarcodeIds = lm.filter(o => o.BarcodeId).map(o => o.BarcodeId);
    const fbs = await this.tl.sharehttp.post<string[]>(`${this.tl.sharebaseUrl}api/Query/AskFormateBarcodes`, { OrgStockUnitId: this.refobj.StockUnitId, FormateNumbrs: FormateNumbrs }).toPromise()
    BarcodeIds = BarcodeIds.concat(fbs);
    const o = { ...this.refobj, PickOrderSubs: BarcodeIds.map(p => ({ BarcodeId: p })) };




    //if (this.env.doProcess) {
    //  o.ProcessDate = new Date();
    //}


    console.log(o);

    await this.cud.add(o);



  }
  //AskFormateBarcodes



  async subdvchange(d) {
    if (d > 0) {
      this.locbarcodes$.next([]);
      this.locMapRs$.next([]);
    }
    const o = await this.tl.sharehttp.get<IIdCount[]>(this.tl.sharebaseUrl + 'api/Query/StockUnitBarcodes/' + d).pipe(take(1)).toPromise();
    this.locbarcodes$.next(o);
    this.xl.preQuery({ OrgStockUnitId: d });
  }

  fitmap(e: IIdCount) {
    let maprs = this.locMapRs$.getValue();
    console.log(e);
    if (e.WithBarcodes) {
      if (e.Selected) {
        maprs = [...maprs, e];
      } else {
        maprs=  this.shiftmap( maprs, e.BarcodeId, e.FormateId )
      }
    } else {
      if (e.SetCount > 0) {
        if (e.SetCount > e.Count) {
          e.SetCount = e.Count;
        }
        const d = maprs.find(o => o.FormateId == e.FormateId);
        if (d) {
          d.SetCount = e.SetCount;
        } else {
          console.log(e);
          maprs = [...maprs, { ...e }];
        }
      } else {
        e.SetCount = 0;
        maprs=this.shiftmap(maprs, e.BarcodeId, e.FormateId)
      }
    }
    this.locMapRs$.next(maprs);
  }
  shiftmap( maprs: IIdCount[], barcodeId: string, formateId: number) {
    if (barcodeId) {
      maprs = maprs.filter(o => o.BarcodeId != barcodeId);
      console.log(maprs);
    } else {
      if (formateId) {
        maprs = maprs.filter(o => o.FormateId != formateId);
      }
    }
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
