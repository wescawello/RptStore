import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
 

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.scss']
})
export class BaseComponent implements OnInit {

  constructor(public tl: TablayoutService) {
    console.log('equipment-cost');
  }
  ngOnInit() {
    this.tl.tabNames$.next({
      query: "撿貨主檔",
      edit: "編輯撿貨",
      add: "新增撿貨",
    });
    this.tl.init("PickOrders");
  }

}
