import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../shared/models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  getUsers() {
    return this.http.get(`${this.apiUrl}/api/UsersManagement`);
  }

  postUser(user: User): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/api/UsersManagement`, JSON.stringify(user));
  }

  deleteUser(userId: string) {
    return this.http.delete(`${this.apiUrl}/api/UsersManagement/${userId}`);
  }

  putUser(user: User, userId: string): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/api/UsersManagement/${userId}`, JSON.stringify(user));
  }

}
