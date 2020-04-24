import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
 

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.scss']
})
export class BaseComponent implements OnInit {

  constructor(public tl: TablayoutService) {
    console.log('formates');
  }
  ngOnInit() {
    console.log('devices');

    this.tl.tabNames$.next({
      query: "規格主檔",
      edit: "編輯規格",
      add: "新增規格",
    });
    this.tl.init("formates");
  }

}
