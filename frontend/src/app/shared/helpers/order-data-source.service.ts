import { DataSource } from '@angular/cdk/collections';
import { Order } from '../models/order';
import { Observable, of } from 'rxjs';


export class OrderDataSource extends DataSource<any> {

  constructor(private _orders: Order[]) {
    super();
  }

  connect(): Observable<Order[]> {
    return of(this._orders);
  }

  disconnect() {
  }
}
