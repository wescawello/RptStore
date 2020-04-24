import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit ,OnDestroy{
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  refobj: any = {};
  private ngUnsubscribe = new Subject();

  constructor(public tl: TablayoutService) {

    tl.preAdd$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(o => {
      console.log(o);
      this.refobj = o;
    });



  }

  ngOnInit() {
  }

}
