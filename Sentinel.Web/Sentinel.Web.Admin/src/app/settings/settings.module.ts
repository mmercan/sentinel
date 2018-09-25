import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { ProductComponent } from './product/product.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { DragulaModule } from 'ng2-dragula';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SettingsRoutingModule,
    SharedModule,
    DragulaModule
  ],
  declarations: [ProductComponent]
})
export class SettingsModule { }
