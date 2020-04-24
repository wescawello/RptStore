import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {
  refobj: any = {};
  
  xvandors$ = new BehaviorSubject<IIdName[]>(null);
  xdevices$ = new BehaviorSubject<IIdName[]>(null);
  xformates$ = new BehaviorSubject<IIdName[]>(null);
  constructor(public tl: TablayoutService, public sconf: SconfService, ) {

    tl.editobj$.subscribe(o => {
      console.log(o);
      this.refobj = { ...o };

      if (this.refobj.ApplyDate) this.refobj.ApplyDate = new Date(this.refobj.ApplyDate);
      if (this.refobj.PurchaseDate) this.refobj.PurchaseDate = new Date(this.refobj.PurchaseDate);
      //PurchaseDate: 

      this.triggerformate();


    }); 
    

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
    let kk = await this.tl.ask<{ Name: string, PurchasePrice: number, DoTimes: boolean, WithBarcodes: boolean }>(k, "formates");

    this.refobj.Formate = { ...kk };
    this.refobj.PurchasePrice = kk.PurchasePrice;


    this.mapbarcodes();
    console.log(this.refobj);
  }
  async mapbarcodes() {
    if (this.refobj.Quantity > 0 && this.refobj.Formate.SetCount) {
      let xlength = this.refobj.Quantity * (this.refobj.Formate.DoTimes ? this.refobj.Formate.SetCount : 1)

      console.log(xlength);

      //let bb = [...new Array(xlength)].map(() => ({ BarcodeValue: "" }));
      let bb = Array.from({ length: xlength }, (v, k) => ({ BarcodeValue: this.refobj.Barcodes[k] ? this.refobj.Barcodes[k].BarcodeValue : "" }));

      console.log(bb);
      this.refobj.Barcodes = bb;
    } else {
      this.refobj.Barcodes = [];
    }



  }
  fillBarcodes() {

    return this.refobj.Barcodes.filter(o => o.BarcodeValue).length;
  }
  novalirefobj(x) {
    return !x.ApplyNumber || !x.ApplyDate || !x.PurchaseDate || !x.Formate || !x.Quantity || !x.PurchasePrice || x.Barcodes.filter(o => !o.BarcodeValue).length > 0;
  }
  async saverefobj() {
    let o = { ...this.refobj };
    delete o.Formate;
    o.Total = o.Quantity * o.PurchasePrice;
    let g = await this.tl.mod(o, o.PurchaseId);
   // console.log(g);
    //this.tl.activatedTabIndex = 0;
  }
}
