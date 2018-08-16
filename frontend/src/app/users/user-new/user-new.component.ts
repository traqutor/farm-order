import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Role } from '../../shared/models/role';
import { User } from '../../shared/models/user';
import { SharedService } from '../../shared/shared.service';
import { PasswordValidation } from '../../shared/validators/match-password.validator';
import { UsersService } from '../users.service';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styleUrls: ['./user-new.component.css']
})
export class UserNewComponent implements OnInit {

  user: FormGroup;
  roles$: Observable<{ results: Array<Role>, resultCount: number }>;
  customers$: Observable<{ results: Array<User>, resultCount: number }>;

  constructor(private sharedService: SharedService,
              private fb: FormBuilder,
              private usersService: UsersService,
              private snackBar: MatSnackBar,
              private router: Router,
              private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.roles$ = this.sharedService.getRoles();
    this.customers$ = this.sharedService.getCustomers();
    this.user = this.fb.group({
      userName: ['', [
        Validators.required,
        Validators.minLength(6),
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
      ]],
      confirmPassword: ['', [
        Validators.required,
        Validators.minLength(6),
      ]],
      customer: ['', [
        Validators.required,
      ]],
      roleId: ['', [
        Validators.required,
      ]]
    }, { validator: PasswordValidation.MatchPassword });
  }

  onSubmit() {
    const { value, valid } = this.user;
    if (valid) {
      this.usersService.postUser(value)
        .subscribe(() => {
          this.router.navigate(['../'], { relativeTo: this.route });
          this.snackBar.open('User Created!', '', {
            duration: 2000,
          });
        }, err => {
          console.log(err);
        });
    }
  }

}
