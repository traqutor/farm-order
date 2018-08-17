import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { User } from './models/user';
import { Role } from './models/role';
import { Customer } from './models/customer';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  getCustomers(): Observable<{results: Array<Customer>, resultCount: number}> {
    return this.http.get<{results: Array<Customer>, resultCount: number}>(`${this.apiUrl}/api/Customer`);
  }

  getRoles(): Observable<{results: Array<Role>, resultCount: number}> {
    return this.http.get<{results: Array<Role>, resultCount: number}>(`${this.apiUrl}/api/Role`);
  }
}
