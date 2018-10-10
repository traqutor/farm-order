import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { Router } from '@angular/router';
import { User } from '../../shared/models/user';
import { distinctUntilChanged } from 'rxjs/operators';
import { SharedService } from '../../shared/shared.service';


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
  private currentUserSubject = new BehaviorSubject<User>({} as User);
  public currentUser = this.currentUserSubject.asObservable().pipe(distinctUntilChanged());

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

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

  setUser(user?: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  setAuth(auth: boolean) {
    this.isAuthenticatedSubject.next(auth);
  }

  getUser(): User {
    return JSON.parse(localStorage.getItem('user'));
  }

  isUserAuthenticated() {
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
    this.setUser({} as User);
    this.setAuth(false);
    this.router.navigateByUrl('/login');
  }

}
