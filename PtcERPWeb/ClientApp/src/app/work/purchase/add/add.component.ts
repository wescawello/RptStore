import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { map, filter, takeUntil } from 'rxjs/operators';
import { Observable, BehaviorSubject, from, Subject } from 'rxjs';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit, OnDestroy{
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  refobj: any = {};

  dropdownSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'VandorId',
    textField: 'Name',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 10,
    allowSearchFilter: true
  };
  mvender$ = new BehaviorSubject<{ VandorId: string, Name: string }[]>([]);
  xvandors$ = new BehaviorSubject<IIdName[]>(null);
  xdevices$ = new BehaviorSubject<IIdName[]>(null);
  xformates$ = new BehaviorSubject<IIdName[]>(null);
  xformatesnob$ = new BehaviorSubject<IIdName[]>(null);
  xstockunits$= new BehaviorSubject<IIdName[]>(null);
  fillBarcodes$ = new BehaviorSubject<number>(0);

  private ngUnsubscribe = new Subject();

  constructor(public tl: TablayoutService, public sconf: SconfService, ) {

    tl.preAdd$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(o => {
      //console.log(o);
      this.refobj = { ...o, Barcodes: <{ BarcodeValue: string, StockUnitId: number }[]>[] };



      //this.refobj.Display = true;
    });
    sconf.mvandor$.pipe(filter(o => o != null), map(o => o.map(p => ({ VandorId: p.Id, Name: p.Name })))).subscribe(o => {

      this.mvender$.next(o);
    });
   // from(this.refobj).subscribe(o => {

    //this.fillBarcodes$.next(this.refobj['Barcodes'].filter(o => o.BarcodeValue).length);
   // });

  }
  nx(event: KeyboardEvent) {
    let elsrc = (<HTMLInputElement>event.srcElement)
    let element = <HTMLInputElement>elsrc.parentElement.parentElement.nextElementSibling.firstElementChild.firstElementChild;
    //console.log('enter');
    if (elsrc.value && this.refobj.Barcodes.filter(o => o.BarcodeValue == elsrc.value).length == 1) {
      if (element == null)  // check if its null
        return;
      else {
        if (event.keyCode == 13) {
          element.focus();   // focus if not null
        }
      }
    } else {
      elsrc.value = "";
      setTimeout(_ =>  elsrc.focus(),200)
    

    }
    this.fillBarcodes$.next(this.refobj['Barcodes'].filter(o => o.BarcodeValue).length);




   
  }


  ngOnInit() {

    this.sconf.xvandors$.pipe(filter(o => o != null)).subscribe(o => this.xvandors$.next(o));
    this.sconf.xdevices$.pipe(filter(o => o != null)).subscribe(o => this.xdevices$.next(o));
  }
  dvchange(x) {
    this.sconf.chvd(x, 'devices').subscribe(o => {
      this.xdevices$.next(o);
    });
    this.triggerformate();
  }
  ddchange(x) {
    this.sconf.chvd(x, 'vandors').subscribe(o => {
      this.xvandors$.next(o);
    });
    this.triggerformate();
  }
  triggerformate() {
    if (this.refobj.VandorId > 0 && this.refobj.DeviceId > 0) {
      this.sconf.chvd([this.refobj.VandorId, this.refobj.DeviceId], 'formates').subscribe(o => {
        this.xformates$.next(o);
      });
    }
  }
 async mapFormate(k) {
   let kk = await this.tl.ask<{Name:string, PurchasePrice: number, DoTimes: boolean, WithBarcodes: boolean }>(k,"formates");

   this.refobj.Formate = { ...kk };
   this.refobj.PurchasePrice = kk.PurchasePrice;


   this.mapbarcodes();
   console.log(this.refobj);
 }
  async mapbarcodes() {
    if (this.refobj.Quantity > 0 && this.refobj.Formate.SetCount) {
      let xlength = this.refobj.Quantity * (this.refobj.Formate.DoTimes ? this.refobj.Formate.SetCount : 1)

      console.log(xlength);  
      let dstr = formatDate(new Date(), 'yyyyMMddhhmmss', 'en-EN');
      let pl = (str, lenght)=>{
        if (str.length >= lenght)
          return str;
        else
          return pl("0" + str, lenght);
      }
      //let bb = [...new Array(xlength)].map(() => ({ BarcodeValue: "" }));
      let bb = Array.from({ length: xlength }, (v, k) => ({ BarcodeValue: this.refobj.Barcodes[k] ? this.refobj.Barcodes[k].BarcodeValue : "", StockUnitId: 1 }));
      if (!this.refobj.Formate.WithBarcodes) {
        bb = Array.from({ length: xlength }, (v, k) => ({ BarcodeValue: `${dstr}${pl(k, 4)}`, StockUnitId: 1 }));
      }
      console.log(bb);  
      this.refobj.Barcodes = bb;




    } else {
      this.refobj.Barcodes = [];
    }

    this.fillBarcodes$.next(this.refobj.Barcodes.filter(o => o.BarcodeValue).length);

  }
  fillBarcodes() {

    return this.refobj.Barcodes.filter(o => o.BarcodeValue).length;
  }
  novalirefobj(x) {
    return !x.ApplyNumber || !x.ApplyDate || !x.PurchaseDate || !x.Formate || !x.Quantity || !x.PurchasePrice || x.Barcodes.length != this.fillBarcodes$.getValue();
  }
  async saverefobj() {
    let o = { ...this.refobj };
    delete o.Formate;
    o.Total = o.Quantity * o.PurchasePrice;
    let g=await this.tl.add(o);
    console.log(g);
    this.tl.activatedTabIndex = 0;
  }

}
