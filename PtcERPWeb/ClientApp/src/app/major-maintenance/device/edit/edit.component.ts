import { Component, OnInit } from '@angular/core';
import { TablayoutService } from '../../../Services/tablayout.service';
import { SconfService } from '../../../shared/sconf.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

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
  }
  lcmod() {
    this.refobj.VandorDevices = this.refobj.VandorDevices.map(vd => ({ ...vd, DeviceId: this.refobj.DeviceId }));
    console.log(this.refobj.VandorDevices);
    this.tl.mod(this.refobj, this.refobj.DeviceId)
  }
}
