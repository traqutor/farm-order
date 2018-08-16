import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { User } from './models/user';
import { Role } from './models/role';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  getCustomers(): Observable<{results: Array<User>, resultCount: number}> {
    return this.http.get<{results: Array<User>, resultCount: number}>(`${this.apiUrl}/api/Customer?page=0`);
  }

  getRoles(): Observable<{results: Array<Role>, resultCount: number}> {
    return this.http.get<{results: Array<Role>, resultCount: number}>(`${this.apiUrl}/api/Role?page=0`);
  }
}
