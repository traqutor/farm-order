import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Role } from '../../shared/models/role';
import { User } from '../../shared/models/user';
import { SharedService } from '../../shared/shared.service';
import { UsersService } from '../users.service';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Customer } from '../../shared/models/customer';
import { Farm } from '../../shared/models/farm';
import { CustomerSite } from '../../shared/models/customer-site';

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
  farms$: Observable<{ results: Array<Farm>, resultCount: number }>;

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
            const customerSitesNullable = oneUser.customer === null ? [] as [CustomerSite] : oneUser.customer.customerSites;
            this.getFarms(customerSitesNullable);
            this.user = this.fb.group({
              id: [oneUser.id],
              userName: [oneUser.userName, [
                Validators.required,
                Validators.minLength(6),
              ]],
              customer: [oneUser.customer, [
                Validators.required,
              ]],
              customerSites: [customerSitesNullable, [
                Validators.required,
              ]],
              farms: [oneUser.farms, [
                Validators.required
              ]],
              roleId: [oneUser.role.id, [
                Validators.required,
              ]]
            });
          });
      });
  }

  getFarms(customerSites: [CustomerSite]) {
    if (this.user) {
      this.user.controls.farms.setValue(null);
    }
    this.farms$ = this.sharedService.getFarms({ page: null, customerSites });
  }

  resetFarms() {
    this.user.controls.farms.setValue(null);
  }

  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  findCustomerSite(arr, val) {
    const item = arr.find(el => el.id === val);
    return item.customerSites;
  }

  onSubmit() {
    const { value, valid } = this.user;
    value.customer.customerSites = value.customerSites;
    delete value.customerSites;
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
