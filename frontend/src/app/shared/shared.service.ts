import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Role } from './models/role';
import { Customer } from './models/customer';
import { Farm } from './models/farm';
import { CustomerSite } from './models/customer-site';
import { User } from './models/user';
import { OrderChangeReason, Status } from './models/order';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  getCustomers(): Observable<{ results: Array<Customer>, resultCount: number }> {
    return this.http.get<{ results: Array<Customer>, resultCount: number }>(`${this.apiUrl}/api/Customer`);
  }

  getRoles(): Observable<{ results: Array<Role>, resultCount: number }> {
    return this.http.get<{ results: Array<Role>, resultCount: number }>(`${this.apiUrl}/api/Role`);
  }

  getFarms(customerSites: { page: number, customerSites: [CustomerSite] }): Observable<{ results: Array<Farm>, resultCount: number }> {
    return this.http.post<{ results: Array<Farm>, resultCount: number }>(`${this.apiUrl}/api/Farm`, JSON.stringify(customerSites));
  }

  getUserAssignedFarms(): Observable<{ results: Array<Farm>, resultCount: number }> {
    return this.http.get<{ results: Array<Farm>, resultCount: number }>(`${this.apiUrl}/api/Farm/GetUserAssignedFarms`);
  }

  getUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/api/Account/UserInfo`);
  }

  getStatus(): Observable<{ results: [Status], resultCount: number }> {
    return this.http.get<{ results: [Status], resultCount: number }>(`${this.apiUrl}/api/OrderStatus`);
  }

  getOrderChangeReason(): Observable<{ results: [OrderChangeReason], resultCount: number }> {
    return this.http.get<{ results: [OrderChangeReason], resultCount: number }>(`${this.apiUrl}/api/OrderChangeReason`);
  }

}
