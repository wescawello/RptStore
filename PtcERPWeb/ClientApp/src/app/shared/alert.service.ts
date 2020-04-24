import { Injectable } from '@angular/core';
import { AlertComponent } from 'ngx-bootstrap/alert';


@Injectable({
  providedIn: 'root'
})
export class AlertService {
  alerts: any[] = [
  //  {
  //  type: 'success',
  //  msg: `Well done! You successfully read this important alert message. (added: ${new Date().toLocaleTimeString()})`,
  //  timeout: 5000
  //}


  ];

  add(s, t: "info" | "success" | "danger" |"warning"): void {
    this.alerts.push({
      type: t,
      msg: `${s} (added: ${new Date().toLocaleTimeString()})`,
      timeout: 5000
    });
  }
  onClosed(dismissedAlert: AlertComponent): void {
    this.alerts = this.alerts.filter(alert => alert !== dismissedAlert);
  }
}
