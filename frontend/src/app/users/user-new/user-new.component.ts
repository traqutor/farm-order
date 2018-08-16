import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Role } from '../../shared/models/role';
import { User } from '../../shared/models/user';
import { SharedService } from '../../shared/shared.service';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styleUrls: ['./user-new.component.css']
})
export class UserNewComponent implements OnInit {

  user: FormGroup;
  roles$: Observable<{ results: Array<Role>, resultCount: number }>;
  customers$: Observable<{ results: Array<User>, resultCount: number }>;

  constructor(private sharedService: SharedService) {
  }

  ngOnInit() {
    this.roles$ = this.sharedService.getRoles();
    this.customers$ = this.sharedService.getCustomers();
    this.user = new FormGroup({
      userName: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern('/\d')]),
      password: new FormControl(''),
      confirmPassword: new FormControl(''),
      customer: new FormControl(''),
      roleId: new FormControl('')
    });
  }

  onSubmit({value, valid}) {
    console.log(value);
  }

}
