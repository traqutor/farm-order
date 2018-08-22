import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Role } from '../shared/models/role';
import { Order } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  getOrders(searchParams): Observable<{ results: Array<Order>, resultCount: number }> {
    return this.http.post<{results: Array<Order>, resultCount: number}>(`${this.apiUrl}/api/Order/Search`, JSON.stringify(searchParams));
  }

  postOrder(order: Order) {
    return this.http.post(`${this.apiUrl}/api/Order`, JSON.stringify(order));
  }

  getOrderById(orderId: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/api/Order/${orderId}`);
  }

  putOrder(orderId: number, order: Order): Observable<Order> {
    return this.http.put<Order>(`${this.apiUrl}/api/Order/${orderId}`, JSON.stringify(order));
  }

}
