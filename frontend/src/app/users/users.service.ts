import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  getUsers() {
    return this.http.get(`${this.apiUrl}/api/UsersManagement?page=0&customerId=0&siteId=0`);
  }

}
