import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { ProductRoutingModule } from './product-routing.module';
import { ListComponent } from './list/list.component';
import { CreateComponent } from './create/create.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@mrtmrcn/ngx-datatable';
@NgModule({
    imports: [
        CommonModule,
        ProductRoutingModule,
        SharedModule,
        NgbModule,
        NgxDatatableModule
    ],
    declarations: [ListComponent, CreateComponent]
})
export class ProductModule { }
