import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
 

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.scss']
})
export class BaseComponent implements OnInit {

  constructor(public tl: TablayoutService) {
    console.log('devices');
  }
  ngOnInit() {
    console.log('devices');
    console.log(this.tl.activatedTabIndex);

    this.tl.tabNames$.next({
      query: "設備主檔",
      edit: "編輯設備",
      add: "新增設備",
    });
    this.tl.init("devices");
  }

}
