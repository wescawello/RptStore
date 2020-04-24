import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';
@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {

  refobj: any = {};
  dropdownSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'VandorId',
    textField: 'Name',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 10,
    allowSearchFilter: true
  };
  xvandors$ = new BehaviorSubject<IIdName[]>(null);
  xdevices$ = new BehaviorSubject<IIdName[]>(null);
  constructor(public tl: TablayoutService,public sconf: SconfService) {
    tl.editobj$.subscribe(o => {
      console.log(o);
      this.refobj = { ...o };
    }); 
  }
  onSelectAll(p) {
    console.log(p);
  }
  onItemSelect(p) {
    console.log(p);
  }
  ngOnInit() {
    this.sconf.xvandors$.pipe(filter(o => o != null)).subscribe(o => this.xvandors$.next(o));
    this.sconf.xdevices$.pipe(filter(o => o != null)).subscribe(o => this.xdevices$.next(o));
  }
  dvchange(x) {
    this.sconf.chvd(x, 'devices').subscribe(o => {
      this.xdevices$.next(o);
    });
  }
  ddchange(x) {
    this.sconf.chvd(x, 'vandors').subscribe(o => {
      this.xvandors$.next(o);
    });
  }

  lcmod() {
    this.refobj.VandorDevices = this.refobj.VandorDevices.map(vd => ({ ...vd, DeviceId: this.refobj.DeviceId }));
    console.log(this.refobj.VandorDevices);
    this.tl.mod(this.refobj, this.refobj.DeviceId)
  }
}
