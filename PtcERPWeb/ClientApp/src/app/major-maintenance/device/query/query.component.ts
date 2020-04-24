import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
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
export class QueryComponent implements OnInit ,OnDestroy {
    ngOnDestroy(): void {
      this.lcpipe.unsubscribe();
    }
  qmodel: any = {};
    oo: boolean;
  smodel: any = null;
  lcResults$ = new BehaviorSubject<any[]>(null);
  lcpipe: Subscription;
  constructor(public tl: TablayoutService, public sconf: SconfService, private ref: ChangeDetectorRef) {
  }
  ngOnInit() {
    this.qmodel.Display = null;
    this.qmodel.TaxStatus = null;

    this.lcpipe = this.tl.pagerObj.Results$.pipe(filter(o => o !== null), map(o => {
      this.sconf.mvandor$.getValue();
      return o.map(p => { return { ...p, vlist: p.VandorDevices.map(q => q.Name).join() } })


    })) .subscribe(o => {

      this.lcResults$.next(o);

    });



  }
  
  action(i, o) {
    this.tl.onSelectTab(null, i);
    this.tl.editobj$.next(o);
  }


  Clear() {
    console.log(this.qmodel);
    this.qmodel = {};
    console.log(this.qmodel);

  }
  search() {

  }
  sortData(sort: Sort) {
    this.tl.setOrder(sort.active, sort.direction);
  }
}
