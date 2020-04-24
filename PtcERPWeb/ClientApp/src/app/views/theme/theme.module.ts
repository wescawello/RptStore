// Angular
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ColorsComponent } from './colors.component';
import { TypographyComponent } from './typography.component';

// Theme Routing
import { ThemeRoutingModule } from './theme-routing.module';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

@NgModule({
  imports: [
    CommonModule,
    ThemeRoutingModule
  ],
  declarations: [
    ColorsComponent,
    TypographyComponent,FetchDataComponent
  ]
})
export class ThemeModule { }
