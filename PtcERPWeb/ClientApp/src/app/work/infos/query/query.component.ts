import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { Sort } from '@angular/material/sort';
import { AlertService } from '../../../shared/alert.service';
import { ModalDirective } from 'ngx-bootstrap/modal/modal.directive';
import { Resful } from '../../../Services/resful';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-query',
  templateUrl: './query.component.html',
  styleUrls: ['./query.component.scss']
})
export class QueryComponent implements OnInit {
  qmodel: any = {
    FormateId: null, StockUnitId: null
  };
  oo: boolean;
  cud: Resful<any,any>;
  delobj: any = {};
  smodel: any = null;
  stockView: Resful<any, unknown>;
  prodView: Resful<any, unknown>;
  prodViewRs$: Observable<unknown>;
  stockViewRs$: Observable<unknown>;
  constructor(public tl: TablayoutService, public sconf: SconfService, private ref: ChangeDetectorRef,public alts: AlertService) {
    this.prodView = this.tl.genResful('InfobyProds', true);
    this.stockView = this.tl.genResful('InfobyStocks', true);
    this.cud = this.tl.genResful('Infos', true);
    this.prodViewRs$ = this.prodView.pagerObj.Results$.pipe(map(rs => {
      return rs.map(r => {
        r.ck = () => {
          this.cud.preQuery({ FormateId: r.FormateId, StockUnitId:null});
        }
        return r;
      })
    }));
    this.stockViewRs$ = this.stockView.pagerObj.Results$.pipe(map(rs => {
      return rs.map(r => {
        r.ck = () => {
          this.cud.preQuery({ FormateId: null, StockUnitId: r.StockUnitId });
        }
        return r;
      })
    }));

    this.prodView.preQuery({});
    this.stockView.preQuery({});

  }
  ngOnInit() {
 
  }
   
  action(i,o) {
    this.tl.onSelectTab(null, i);
    this.tl.editobj$.next(o);
   
  }
 
  async del(e) {
    const j = await this.tl.del(e);
    this.alts.add(`${j.ApplyNumber}刪除完成`, "success");
    this.tl.prequery(this.qmodel);
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
    this.cud.preQuery(this.qmodel);

  }
  async search() {
   // InfoHisSave

    const pp = this.tl.sharehttp.get(this.tl.sharebaseUrl + 'api/Query/InfoHisSave').toPromise();
    console.log(pp);
  }
  sortData(sort: Sort) {
    this.tl.setOrder(sort.active, sort.direction);
  }
}
