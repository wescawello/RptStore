import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { Sort } from '@angular/material/sort';
import { AlertService } from '../../../shared/alert.service';
import { ModalDirective } from 'ngx-bootstrap/modal/modal.directive';
import { Resful } from '../../../Services/resful';

@Component({
  selector: 'app-query',
  templateUrl: './query.component.html',
  styleUrls: ['./query.component.scss']
})
export class QueryComponent implements OnInit {
  qmodel: any = {};
  oo: boolean;
  delId= "";
  delApplyNumber = "";
  smodel: any = null;
  cud: Resful<any,any>;
  constructor(public tl: TablayoutService, public sconf: SconfService, private ref: ChangeDetectorRef, public alts: AlertService) {
    this.cud = this.tl.genResful('StockOrders',true);

  }
  ngOnInit() {
    this.qmodel.Status = null;
    this.qmodel.TaxStatus = null;
  }
  
  action(i,o) {
    this.tl.onSelectTab(null, i);
    this.tl.editobj$.next(o);
   // this.tl.oduserIdName([o.CreateId,o.UpdateId]);



  }
  async execProcess(e) {
    let b = await this.tl.execProcess(e);
  }
  async del(e) {
    const j = await this.cud.del(e);
    this.alts.add(`${j.ApplyNumber}刪除完成`, "success");
    //this.tl.syncq$.next({});
  }
  handler(type: string, $event: ModalDirective) {
    console.log(
      `event ${type} is fired${$event.dismissReason
        ? ', dismissed by ' + $event.dismissReason
        : ''}`
    );
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