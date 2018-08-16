import { Component, OnInit } from '@angular/core';
import { UsersService } from '../users.service';
import { SharedService } from '../../shared/shared.service';
import { Observable } from 'rxjs';
import { User } from '../../shared/models/user';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {

  customers$: Observable<{ results: Array<User>, resultCount: number }>;
  dataSource;

  constructor(private usersService: UsersService,
              private sharedService: SharedService) {
  }

  ngOnInit() {
    this.usersService.getUsers().subscribe(res => {
      console.log(res);
    });
    this.customers$ = this.sharedService.getCustomers();
  }

}
