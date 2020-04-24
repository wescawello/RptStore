import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { BehaviorSubject, Observable, Subscription, Subject } from 'rxjs';
import { take, map, filter, takeUntil, skip } from 'rxjs/operators';
import { AlertService } from '../../../shared/alert.service';
import { Router } from '@angular/router';
import { PageChangedEvent } from 'ngx-bootstrap/pagination/ngx-bootstrap-pagination';
import { IPagerRs } from '../../../Models/Ipager-rs';
import { Resful } from '../../../Services/resful';
import { isNumber } from 'util';
import { BsDatepickerViewMode } from 'ngx-bootstrap/datepicker/models';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit,OnDestroy {
  
  qmodel: any = {};
  ngOnDestroy(): void {
   
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  bsConfig: Partial< BsDatepickerConfig>;
  private ngUnsubscribe = new Subject();
  constructor(public tl: TablayoutService, public sconf: SconfService, private alts: AlertService, router: Router) {
    tl.preAdd$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(async o => {
      const allprocess = await this.tl.sharehttp.get<boolean>(this.tl.sharebaseUrl + 'api/Query/AllStockOrderProsess/').pipe(take(1)).toPromise();
      if (!allprocess) {
        console.log(allprocess);
        tl.onSelectTab(null, 0);
        alts.add("有未執行的轉倉單須先執行", "danger");
      } else {
        const dnow = new Date()
        this.qmodel.CreateYM = dnow;
        this.qmodel.CreateYMstr = `${dnow.getFullYear()}${dnow.getMonth() < 10 ? '0' : ''}${dnow.getMonth() + 1}`
      }
    });
    const minMode: BsDatepickerViewMode = 'month';
    this.bsConfig = {
      minMode: minMode,
      dateInputFormat: 'YYYYMM'
    };

  


  }
   


   
 
  
  ngOnInit() {
    
  }
  // fired every time search string is changed
   
}
