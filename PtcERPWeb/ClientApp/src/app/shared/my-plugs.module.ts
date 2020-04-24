import { NgModule, ModuleWithProviders } from '@angular/core';
import { MyMaterialModule } from './my-material/my-material.module';
import { MyNgxBootstrapModule } from './my-ngx-bootstrap/my-ngx-bootstrap.module';
import { SortpropComponent } from './sortprop/sortprop.component';
import { CommonModule } from '@angular/common';
//import { TabctrlComponent } from '../tabctrl/tabctrl.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
 
 
const list = [
  CommonModule ,
  MyMaterialModule ,
  MyNgxBootstrapModule,
  FontAwesomeModule,
]
const listex = [
  MyMaterialModule,
  MyNgxBootstrapModule,
  FontAwesomeModule,

  SortpropComponent,
  
]

@NgModule({
  imports: list,
  exports: listex,
  declarations: [SortpropComponent]
})
export class MyPlugsModule {
  //static forRoot(providers = []): ModuleWithProviders {
  //  return {
  //    ngModule: MyPlugsModule,
  //    providers: [...providers]
  //  }
  //}
}
