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
      query: "退貨主檔",
      edit: "檢視退貨",
      add: "新增退貨",
    });
    this.tl.activatedTabIndex = 0;
    this.tl.addable = true;
    this.tl.editable = false;
  }

}
