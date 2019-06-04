import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { ProductRoutingModule } from './product-routing.module';
import { ListComponent } from './list/list.component';
import { CreateComponent } from './create/create.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { FormsModule } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { DynamicHTMLModule } from '../dynamic-html/dynamic-html.module';
@NgModule({
    imports: [
        CommonModule,
        ProductRoutingModule,
        SharedModule,
        NgbModule,
        NgxDatatableModule,  // (https://github.com/swimlane/ngx-datatable/issues/1693)
        FormsModule,
        ScrollingModule,
        DynamicHTMLModule
    ],
    declarations: [ListComponent, CreateComponent]
})
export class ProductModule { }
