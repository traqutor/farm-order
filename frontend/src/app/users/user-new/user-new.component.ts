import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';
import { Role } from '../../shared/models/role';
import { SharedService } from '../../shared/shared.service';
import { PasswordValidation } from '../../shared/validators/match-password.validator';
import { UsersService } from '../users.service';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Customer } from '../../shared/models/customer';
import { CustomerSite } from '../../shared/models/customer-site';
import { Farm } from '../../shared/models/farm';
import { DialogService } from '../../shared/dialogs/dialog.service';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styleUrls: ['./user-new.component.css']
})
export class UserNewComponent implements OnInit {

  user: FormGroup;
  roles$: Observable<{ results: Array<Role>, resultCount: number }>;
  customers$: Observable<{ results: Array<Customer>, resultCount: number }>;
  farms$: Observable<{ results: Array<Farm>, resultCount: number }>;

  constructor(private sharedService: SharedService,
              private fb: FormBuilder,
              private usersService: UsersService,
              private snackBar: MatSnackBar,
              private router: Router,
              private route: ActivatedRoute,
              private _location: Location,
              private dialogService: DialogService) {
  }

  ngOnInit() {
    this.roles$ = this.sharedService.getRoles();
    this.customers$ = this.sharedService.getCustomers();
    this.user = this.fb.group({
      userName: [null, [
        Validators.required,
        Validators.minLength(6),
      ]],
      password: [null, [
        Validators.required,
        Validators.minLength(6),
      ]],
      confirmPassword: [null, [
        Validators.required,
        Validators.minLength(6),
      ]],
      customer: [null, [
        Validators.required,
      ]],
      customerSites: [null, [
        Validators.required,
      ]],
      farms: [null, [
        Validators.required
      ]],
      roleId: [null, [
        Validators.required,
      ]]
    }, { validator: PasswordValidation.MatchPassword });
  }

  getFarms(customerSites: [CustomerSite]) {
    this.user.controls.farms.setValue(null);
    this.farms$ = this.sharedService.getFarms({ page: null, customerSites });
  }

  resetFarms() {
    this.user.controls.farms.setValue(null);
  }

  onSubmit() {
    const { value, valid } = this.user;
    value.customer.customerSites = value.customerSites;
    delete value.customerSites;
    if (valid) {
      this.usersService.postUser(value)
        .subscribe(() => {
          this.router.navigate(['../'], { relativeTo: this.route });
          this.snackBar.open('User Created!', '', {
            duration: 2000,
          });
        }, err => {
          this.dialogService.alert(err.error);
        });
    }
  }
  cancel() {
    this._location.back();
  }

}
