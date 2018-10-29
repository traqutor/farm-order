import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable} from 'rxjs';
import {IMultipleOrder, IOrder} from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  getOrders(searchParams): Observable<{ results: Array<IOrder>, resultsCount: number }> {
    return this.http.post<{ results: Array<IOrder>, resultsCount: number }>(`${this.apiUrl}/api/Order/Search`, searchParams);
  }

  postOrder(order: IOrder) {
    return this.http.post(`${this.apiUrl}/api/Order`, order);
  }

  getOrderById(orderId: number): Observable<IOrder> {
    return this.http.get<IOrder>(`${this.apiUrl}/api/Order/${orderId}`);
  }

  putOrder(orderId: number, order: IOrder): Observable<IOrder> {
    return this.http.put<IOrder>(`${this.apiUrl}/api/Order/${orderId}`, order);
  }

  deleteOrderById(orderId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/Order/${orderId}`);
  }

  putMultipleOrder(order: IMultipleOrder) {
    return this.http.post(`${this.apiUrl}/api/Order/CreateMultiple`, order);

  }

}
