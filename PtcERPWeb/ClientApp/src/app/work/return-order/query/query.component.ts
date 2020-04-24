import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { AlertService } from '../../../shared/alert.service';
import { ModalDirective } from 'ngx-bootstrap/modal/modal.directive';
import { Resful } from '../../../Services/resful';

@Component({
  selector: 'app-query',
  templateUrl: './query.component.html',
  styleUrls: ['./query.component.scss']
})
export class QueryComponent implements OnInit {
  qmodel: any = {
    Customer: { CustomerInfoId:0}
  };
  oo: boolean;
  delId= "";
  delApplyNumber = "";
  cud: Resful<RReturnOrder,any>;
  constructor(public tl: TablayoutService, public sconf: SconfService, private ref: ChangeDetectorRef, public alts: AlertService) {
    this.cud = this.tl.genResful<RReturnOrder,any>('ReturnOrders', true);
  }
  ngOnInit() { 
    console.log("ReturnOrders");
  }
  async action(i, o) {
    const q = await this.cud.find(o.ReturnOrderId);
    this.tl.editobj$.next(q);
    this.tl.onSelectTab(null, i);

  }
  async del(e) {
    const d = await this.cud.del(e);
    console.log(d);
    this.alts.add(`${e.ApplyNumber}刪除完成`, "success");
    this.tl.syncq$.next({})
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
    this.qmodel = {
      Customer: {}};
    console.log(this.qmodel);

  }
  
}
