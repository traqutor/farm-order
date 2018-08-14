import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersListComponent } from './orders-list/orders-list.component';

@NgModule({
  imports: [
    CommonModule
  ],
  exports: [OrdersListComponent],
  declarations: [OrdersListComponent]
})
export class OrdersModule { }
