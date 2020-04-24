import { Directive  ,HostListener, Input} from '@angular/core';

@Directive({
  selector: '[href]'
})
export class MyInhertLinkDirective {

  @Input() public href: string | undefined;

  @HostListener('click', ['$event']) public onClick(event: Event): void {
    if (!this.href || this.href === '#' || (this.href && this.href.length === 0) || this.href.includes("javascript:void(0)")) {
      //console.log("ss");
      event.preventDefault();
    }
  }

}
