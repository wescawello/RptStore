import { NgModule  } from '@angular/core';
//import { ModalModule,  PaginationModule, TabsModule, AlertModule, BsDatepickerModule, BsDropdownModule, TypeaheadModule } from 'ngx-bootstrap';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { CommonModule } from '@angular/common';

//const list = [

//  ModalModule,
//  AccordionModule,
//  PaginationModule,
//  BsDatepickerModule ,
//  TabsModule,
//  AlertModule,
//  BsDropdownModule ,
//];
//const listforRoot = list.map(o => o.forRoot());
//let ff = [CommonModule,
//  ModalModule.forRoot(),
//  AccordionModule.forRoot(),
//  PaginationModule.forRoot(),
//  BsDatepickerModule.forRoot(),
//  TabsModule.forRoot(),
//  AlertModule.forRoot(),
//  BsDropdownModule.forRoot(),

//];



@NgModule({
  imports: [CommonModule,
    ModalModule.forRoot(),
    AccordionModule.forRoot(),
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TabsModule.forRoot(),
    AlertModule.forRoot(),
    BsDropdownModule.forRoot(),
    TypeaheadModule.forRoot(),
  ],
  exports: [ModalModule,
    AccordionModule,
    PaginationModule,
    BsDatepickerModule,
    TabsModule,
    AlertModule,
    TypeaheadModule,
    BsDropdownModule]
})
export class MyNgxBootstrapModule {
  //static forRoot(providers = []): ModuleWithProviders {
  //  return {
  //    ngModule: MyNgxBootstrapModule,
  //    providers: [...providers]
  //  }
  //}
}
