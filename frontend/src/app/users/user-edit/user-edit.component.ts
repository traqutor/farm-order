import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Role } from '../../shared/models/role';
import { User } from '../../shared/models/user';
import { SharedService } from '../../shared/shared.service';
import { UsersService } from '../users.service';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { PasswordValidation } from '../../shared/validators/match-password.validator';
import { filter } from 'rxjs/operators';
import { Customer } from '../../shared/models/customer';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  user: FormGroup;
  roles$: Observable<{ results: Array<Role>, resultCount: number }>;
  userId: string;
  customers$: Observable<{ results: Array<Customer>, resultCount: number }>;

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
    this.route
      .params
      .subscribe(params => {
        this.userId = params['id'];
        this.usersService.getUsers()
          .subscribe((res: { results: [User], resultsCount: number }) => {
            const oneUser = res.results.find((user: User) => user.id === this.userId);
            this.user = this.fb.group({
              id: [oneUser.id],
              userName: [oneUser.userName, [
                Validators.required,
                Validators.minLength(6),
              ]],
              customer: [oneUser.customer, [
                Validators.required,
              ]],
              roleId: [oneUser.role.id, [
                Validators.required,
              ]]
            });
          });
      });
  }

  compare(val1, val2) {
    return val1.id === val2.id;
  }

  onSubmit() {
    const { value, valid } = this.user;
    if (valid) {
      this.usersService.putUser(value, this.userId)
        .subscribe(() => {
          this.snackBar.open('User Edited!', '', {
            duration: 2000,
          });
        }, err => {
          console.log(err);
        });
    }
  }

}
