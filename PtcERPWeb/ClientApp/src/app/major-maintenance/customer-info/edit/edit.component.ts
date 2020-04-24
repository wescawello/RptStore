import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {

  refobj: any = {};
  constructor(public tl: TablayoutService) {
    tl.editobj$.subscribe(o => {
      console.log(o);
      this.refobj = { ...o };
    }); 
  }
  ngOnInit() {
  }

}
