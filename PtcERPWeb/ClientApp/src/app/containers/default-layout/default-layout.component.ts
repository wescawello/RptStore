import {Component } from '@angular/core';
import { navItems } from '../../_nav';
import { AlertService } from '../../shared/alert.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  public sidebarMinimized = false;
  public navItems = navItems;
  constructor(public alts: AlertService) {

  }
  toggleMinimize(e) {
    //console.log(e);
    this.sidebarMinimized = e;
  }
}
