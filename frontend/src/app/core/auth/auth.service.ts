import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';


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

  constructor(private http: HttpClient) {
    this.apiUrl = environment.url;
  }

  saveCredentialsToStorage(login: string, token: string) {
    localStorage.setItem('login', login);
    localStorage.setItem('token', token);
  }

  login(credentials): Observable<Object> {
    const { login, password } = credentials;
    const body = new HttpParams()
      .set('grant_type', 'password')
      .set('userName', login)
      .set('password', password);
    return this.http.post(this.apiUrl + '/token', body.toString());
  }

}
