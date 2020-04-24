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
import { Ng5SliderModule } from 'ng5-slider';



@NgModule({
  declarations: [AddComponent, EditComponent, QueryComponent, BaseComponent],
  imports: [
    CommonModule,  FormsModule, ReactiveFormsModule,
    MyPlugsModule,
    Ng5SliderModule,


    RouterModule.forChild([{ path: "", data: { title: "護運作業" }, children: [{ path: "", data: { title: "進貨" }, component: BaseComponent, canActivate: [AuthorizeGuard]} ]}])
  ]
})
export class MainModule { }
