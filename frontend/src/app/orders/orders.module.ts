import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { OrdersListComponent } from './orders-list/orders-list.component';
import { AppMaterialModule } from '../app-material.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OrderNewComponent } from './order-new/order-new.component';
import { OrderEditComponent } from './order-edit/order-edit.component';

@NgModule({
  imports: [
    CommonModule,
    AppMaterialModule,
    FlexLayoutModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [DatePipe],
  exports: [OrdersListComponent],
  declarations: [OrdersListComponent, OrderNewComponent, OrderEditComponent]
})
export class OrdersModule {
}
