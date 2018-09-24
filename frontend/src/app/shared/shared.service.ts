import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable} from 'rxjs';
import {Role} from './models/role';
import {Customer} from './models/customer';
import {Farm} from './models/farm';
import {CustomerSite} from './models/customer-site';
import {User} from './models/user';
import {OrderChangeReason, Status} from './models/order';
import {Ration} from './models/ration';
import {IShed} from "./models/shed";
import {ISilo} from "./models/silo";

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

  getUserAssignedFarms(page: number): Observable<{ results: Array<Farm>, resultsCount: number }> {
    return this.http.get<{ results: Array<Farm>, resultsCount: number }>(`${this.apiUrl}/api/Farm/GetUserAssignedFarms?page=${page}`);
  }

  getUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/api/Account/UserInfo`);
  }

  getStatus(): Observable<{ results: [Status], resultsCount: number }> {
    return this.http.get<{ results: [Status], resultsCount: number }>(`${this.apiUrl}/api/OrderStatus`);
  }

  getOrderChangeReason(): Observable<{ results: [OrderChangeReason], resultsCount: number }> {
    return this.http.get<{ results: [OrderChangeReason], resultsCount: number }>(`${this.apiUrl}/api/OrderChangeReason`);
  }

  resetPassword(credentials) {
    return this.http.post(`${this.apiUrl}/api/Account/ChangePassword`, JSON.stringify(credentials));
  }

  getRations(farmId: number): Observable<{ results: [Ration], resultsCount: number }> {
    return this.http.get<{ results: [Ration], resultsCount: number }>(`${this.apiUrl}/api/Ration?farmId=${farmId}`);
  }

  getSheds(farmId: number, page: number): Observable<{ results: [IShed], resultsCount: number }> {
    return this.http.get<{ results: [IShed], resultsCount: number }>(`${this.apiUrl}/api/Shed?farmId=${farmId}&page=${page}`);
  }

  getSilos(sheds: Array<IShed>, page: number): Observable<{ results: [ISilo], resultsCount: number }> {
    return this.http.post<{ results: [ISilo], resultsCount: number }>(`${this.apiUrl}/api/Silo`, {
      sheds: sheds,
      page: page
    });
  }


}
