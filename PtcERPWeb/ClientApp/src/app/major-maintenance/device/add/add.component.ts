import { Component, OnInit, OnDestroy } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { map, filter, takeUntil } from 'rxjs/operators';
import { Observable, BehaviorSubject, Subject } from 'rxjs';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent implements OnInit, OnDestroy {
    ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
    }
  refobj: any = {};

  dropdownSettings : IDropdownSettings = {
    singleSelection: false,
    idField: 'VandorId',
    textField: 'Name',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 10,
    allowSearchFilter: true
  };
  mvender$=new BehaviorSubject<{VandorId:string,Name:string}[]>([]);
  private ngUnsubscribe = new Subject();
  constructor(public tl: TablayoutService, public sconf: SconfService, ) {

    tl.preAdd$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(o => {
      console.log(o);
      this.refobj = o;
      this.refobj.Display = true;
    });
    sconf.mvandor$.pipe(filter(o => o != null), map(o => o.map(p => ({ VandorId: p.Id, Name: p.Name })))).subscribe(o => {

      this.mvender$.next(o);
    });


  }
  onSelectAll(p) {
    console.log(p);
  }
  onItemSelect(p) {
    console.log(p);
  }
  ngOnInit() {





  }

}
