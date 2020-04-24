import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
 

@Component({
  selector: 'app-base',
  templateUrl: './base.component.html',
  styleUrls: ['./base.component.scss']
})
export class BaseComponent implements OnInit {

  constructor(public tl: TablayoutService) {
    console.log('Infos');
  }
  ngOnInit() {
    this.tl.tabNames$.next({
      query: "總庫存現狀",
      edit: "歷庫史存",
      add: "歷庫史存",
    });
    this.tl.init("Infos");
  }

}
