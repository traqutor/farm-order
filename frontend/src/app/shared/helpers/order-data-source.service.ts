import { DataSource } from '@angular/cdk/collections';
import { Order } from '../models/order';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { MatTableDataSource } from '@angular/material';


export class OrderDataSource extends MatTableDataSource<any> {

  constructor(private _orders: Order[]) {
    super();
  }

  connect(): BehaviorSubject<Order[]> {
    return new BehaviorSubject<Order[]>(this._orders);
  }

  disconnect() {
  }
}
