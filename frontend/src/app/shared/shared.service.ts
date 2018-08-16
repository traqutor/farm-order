import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { User } from './models/user';

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
}
