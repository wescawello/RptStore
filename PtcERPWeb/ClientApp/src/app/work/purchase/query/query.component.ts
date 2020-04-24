import { Component, OnInit, ChangeDetectorRef, OnDestroy, EventEmitter } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { Sort } from '@angular/material/sort';
import { map, filter } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';


@Component({
  selector: 'app-query',
  templateUrl: './query.component.html',
  styleUrls: ['./query.component.scss']
})
export class QueryComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    //this.lcpipe.unsubscribe();
  }
  qmodel: any = { TotalMin: 0,TotalMax:1000};
  oo: boolean;
  smodel: any = null;
  lcResults$ = new BehaviorSubject<any[]>(null);
  lcpipe: Subscription;
  xvandors$ = new BehaviorSubject<IIdName[]>(null);
  xdevices$ = new BehaviorSubject<IIdName[]>(null);
  xformates$ = new BehaviorSubject<IIdName[]>(null);

  slidersRefresh: EventEmitter<void> = new EventEmitter<void>();

  constructor(public tl: TablayoutService, public sconf: SconfService, private ref: ChangeDetectorRef) {
  }
  ngOnInit() {
    this.qmodel.Display = null;
    this.qmodel.TaxStatus = null;
    this.qmodel.SalePriceMax = 1000;
    this.sconf.xvandors$.pipe(filter(o => o != null)).subscribe(o => this.xvandors$.next(o));
    this.sconf.xdevices$.pipe(filter(o => o != null)).subscribe(o => this.xdevices$.next(o));


 




    //this.lcpipe = this.tl.pagerObj.Results$.pipe(filter(o => o != null), map(o => {
    //  this.sconf.mvender$.getValue();
    //  return o.map(p => { return { ...p, vlist: p.VandorDevices.map(q => q.Name).join() } })


    //})).subscribe(o => {

    //  this.lcResults$.next(o);

    //});
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
    if (this.qmodel.VandorId > 0 && this.qmodel.DeviceId > 0) {
      this.sconf.chvd([this.qmodel.VandorId, this.qmodel.DeviceId], 'formates').subscribe(o => {
        this.xformates$.next(o);
      });
    }
  }

  ff(d) {
    let gg = d.ApplyDateRange[1]
    var dat = new Date(d.ApplyDateRange[0]); // (1)
    dat.setDate(dat.getDate() + 3);  // (2)
    this.qmodel.ApplyDateRange = [dat, gg];

    //this.oo = true;
    //this.ref.detach();
    // console.log(dat);
    // setTimeout(() => {
    //   //this.oo = false;
    //   this.ref.detectChanges(); 

    // }, 500);

  }
  action(i, o) {
    this.tl.onSelectTab(null, i);
    this.tl.editobj$.next(o);
  }



  Clear() {
    console.log(this.qmodel);
    this.qmodel = { TotalMin: 0, TotalMax: 1000 };
 

  }
  search() {

  }
  sortData(sort: Sort) {
    this.tl.setOrder(sort.active, sort.direction);
  }
}
