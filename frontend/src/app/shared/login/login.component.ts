import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor() {
  }

  onSignIn(form: NgForm) {
    const login = form.value.login;
    const password = form.value.password;
    // this.authService.signInUser(login, password);
  }

  ngOnInit() {
  }

}
