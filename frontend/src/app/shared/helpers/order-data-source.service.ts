import { DataSource } from '@angular/cdk/collections';
import { IOrder } from '../models/order';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { MatTableDataSource } from '@angular/material';


export class OrderDataSource extends MatTableDataSource<any> {

  constructor(private _orders: IOrder[]) {
    super();
  }

  connect(): BehaviorSubject<IOrder[]> {
    return new BehaviorSubject<IOrder[]>(this._orders);
  }

  disconnect() {
  }
}
