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
      query: "廠商主檔",
      edit: "編輯廠商",
      add: "新增廠商",
    });
    this.tl.init("vandors");
  }

}
