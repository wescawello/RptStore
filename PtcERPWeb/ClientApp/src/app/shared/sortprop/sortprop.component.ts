import { Component, OnInit, Input } from '@angular/core';
import { TablayoutService } from '../../Services/tablayout.service';
import { faSortDown, faSortUp,faSort} from '@fortawesome/free-solid-svg-icons';
import { Resful } from '../../Services/resful';
//import {  } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-sortprop',
  templateUrl: './sortprop.component.html',
  styleUrls: ['./sortprop.component.scss']
})
export class SortpropComponent implements OnInit {
  @Input() propname: string;
  @Input() displayname: string;
  @Input() prs: Resful<any,any>;
  faSortDown = faSortDown
  faSortUp = faSortUp
  faSort = faSort;
  qorder = {};
  constructor(public tl: TablayoutService) {
    
  }
  setOrder(s, o) {
    if (this.prs) {
      this.prs.setOrder(s, o);
      console.log(this.prs.qorder);
      this.qorder = { ...this.prs.qorder };
    } else {
      this.tl.setOrder(s, o);
      this.qorder = { ...this.tl.qorder };

    }
  }
  ngOnInit() {
    if (!this.propname) {
      this.propname = this.displayname;
    }
    if (!this.displayname) {
      this.displayname = this.propname;
    }
  }

}
