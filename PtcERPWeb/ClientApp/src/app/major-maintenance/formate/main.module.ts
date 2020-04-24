import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddComponent } from './add/add.component';
import { EditComponent } from './edit/edit.component';
import { QueryComponent } from './query/query.component';
import { RouterModule } from '@angular/router';
 
import { MyPlugsModule } from '../../shared/my-plugs.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BaseComponent } from './base/base.component';
import { AuthorizeGuard } from '../../../api-authorization/authorize.guard';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { Ng5SliderModule } from 'ng5-slider';

@NgModule({
  declarations: [AddComponent, EditComponent, QueryComponent, BaseComponent],
  imports: [
    CommonModule,  FormsModule, ReactiveFormsModule,
    MyPlugsModule,
    NgMultiSelectDropDownModule,
    Ng5SliderModule,


    RouterModule.forChild([{ path: "", data: { title: "主檔維護" }, children: [{ path: "", data: { title: "規格" }, component: BaseComponent, canActivate: [AuthorizeGuard]} ]}])
  ]
})
export class FormateModule { }