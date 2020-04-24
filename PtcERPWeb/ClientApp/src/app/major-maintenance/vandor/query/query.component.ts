import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { Sort } from '@angular/material/sort';

@Component({
  selector: 'app-query',
  templateUrl: './query.component.html',
  styleUrls: ['./query.component.scss']
})
export class QueryComponent implements OnInit {
  qmodel: any = {};
    oo: boolean;
  smodel: any = null;
  constructor(public tl: TablayoutService, public sconf: SconfService, private ref: ChangeDetectorRef) {
  }
  ngOnInit() {
    this.qmodel.Status = null;
    this.qmodel.TaxStatus = null;
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
    this.qmodel = {};
    console.log(this.qmodel);

  }
  search() {

  }
  sortData(sort: Sort) {
    this.tl.setOrder(sort.active, sort.direction);
  }
}
