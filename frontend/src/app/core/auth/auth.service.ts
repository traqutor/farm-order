import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';


export interface AuthResponse {
  '.expires': string;
  '.issued': string;
  access_token: string;
  expires_in: number;
  token_type: string;
  userName: string;
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  apiUrl: string;

  constructor(private http: HttpClient,
              private router: Router) {
    this.apiUrl = environment.url;
  }

  saveCredentialsToStorage(login: string, token: string) {
    localStorage.setItem('login', login);
    localStorage.setItem('token', token);
  }

  removeCredentialsFromStorage() {
    localStorage.removeItem('login');
    localStorage.removeItem('token');
  }

  getTokenFromStorage() {
    const token = localStorage.getItem('token');
    if (token) {
      return token;
    }
    return false;
  }

  isAuthenticated() {
    const token = this.getTokenFromStorage();
    if (token) {
      return true;
    }
    return false;
  }

  login(credentials): Observable<Object> {
    const { login, password } = credentials;
    const body = new HttpParams()
      .set('grant_type', 'password')
      .set('userName', login)
      .set('password', password);
    return this.http.post(this.apiUrl + '/token', body.toString());
  }

  logout() {
    this.removeCredentialsFromStorage();
    this.router.navigateByUrl('/login');
  }

}
