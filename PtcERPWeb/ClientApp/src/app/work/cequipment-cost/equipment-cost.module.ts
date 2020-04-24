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



@NgModule({
  declarations: [AddComponent, EditComponent, QueryComponent, BaseComponent],
  imports: [
    CommonModule,  FormsModule, ReactiveFormsModule,
    MyPlugsModule,



    RouterModule.forChild([{ path: "", component: BaseComponent, canActivate: [AuthorizeGuard] }])
  ]
})
export class EquipmentCostModule { }
