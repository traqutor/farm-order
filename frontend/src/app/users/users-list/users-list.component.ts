import { Component, OnInit } from '@angular/core';
import { UsersService } from '../users.service';
import { SharedService } from '../../shared/shared.service';
import { Observable } from 'rxjs';
import { User } from '../../shared/models/user';
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {

  customers$: Observable<{ results: Array<User>, resultCount: number }>;
  dataSource = new MatTableDataSource<User>([]);
  displayedColumns = ['id', 'userName', 'customer', 'role'];

  constructor(private usersService: UsersService,
              private sharedService: SharedService) {
  }

  ngOnInit() {
    this.usersService.getUsers()
      .subscribe((users: { results: [User], resultCount: number }) => {
        this.dataSource = new MatTableDataSource<User>(users.results);
      });
    this.customers$ = this.sharedService.getCustomers();
  }

  displayRow(row) {
    if (typeof row === 'object' && row !== null && row.name !== null) {
      return row.name;
    }
    return row;
  }

}
