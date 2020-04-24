import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
 

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.scss']
})
export class BaseComponent implements OnInit {

  constructor(public tl: TablayoutService) {
    console.log('SaleOrders');
  }
  ngOnInit() {
    this.tl.tabNames$.next({
      query: "售貨主檔",
      edit: "編輯售貨",
      add: "新增售貨",
    });
    this.tl.init("SaleOrders");
  }

}
