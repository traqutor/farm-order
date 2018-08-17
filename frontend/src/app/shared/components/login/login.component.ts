import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthResponse, AuthService } from '../../../core/auth/auth.service';
import { tap } from 'rxjs/operators';
import { Router } from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public err = false;
  public errMessage = '';
  public isLogging = false;

  constructor(private authService: AuthService,
              private router: Router) {
  }

  onSignIn(form: NgForm) {
    this.isLogging = true;
    const login = form.value.login;
    const password = form.value.password;
    this.authService.login({ login, password })
      .pipe(
        tap((authResponse: AuthResponse) => {
          this.isLogging = false;
          this.authService.saveCredentialsToStorage(authResponse.userName, authResponse.access_token)
        })
      )
      .subscribe(() => {
        this.router.navigateByUrl('orders');
      }, err => {
        this.isLogging = false;
        this.err = true;
        this.errMessage = err.error.error_description;
      });
  }

  ngOnInit() {
  }

}
