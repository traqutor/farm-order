import { Component, OnInit } from '@angular/core';
import { UsersService } from '../users.service';
import { SharedService } from '../../shared/shared.service';
import { Observable } from 'rxjs';
import { User } from '../../shared/models/user';
import { MatSelect, MatSnackBar, MatTableDataSource } from '@angular/material';
import { DialogService } from '../../shared/dialogs/dialog.service';
import { Customer } from '../../shared/models/customer';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {

  customers$: Observable<{ results: Array<Customer>, resultCount: number }>;
  dataSource = new MatTableDataSource<User>([]);
  displayedColumns = [
    { value: 'id', name: 'Id' },
    { value: 'userName', name: 'UserName' },
    { value: 'customer', name: 'Customer' },
    { value: 'role', name: 'Role' },
  ];
  columnsToRender = ['id', 'userName', 'customer', 'role', 'settings'];

  constructor(private usersService: UsersService,
              private sharedService: SharedService,
              private dialogService: DialogService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit() {
    this.usersService.getUsers()
      .subscribe((users: { results: [User], resultCount: number }) => {
        this.dataSource = new MatTableDataSource<User>(users.results);
        this.dataSource.filterPredicate = (data: User, filter: string) => {
          if (data.customer) {
            return data.customer.name.indexOf(filter) !== -1;
          }
        };
      });
    this.customers$ = this.sharedService.getCustomers();
  }

  displayRow(row) {
    if (typeof row === 'object' && row !== null && row.name !== null) {
      return row.name;
    }
    return row;
  }

  filterTableByCustomer(selectOption: MatSelect) {
    this.dataSource.filter = selectOption.value.name;
  }

  deleteUser(userId: string) {
    this.dialogService
      .confirm('Confirm Action', 'Are you sure you wanna delete User')
      .subscribe(dialogRes => {
        if (dialogRes) {
          this.usersService.deleteUser(userId)
            .subscribe(() => {
              const newUsers = this.dataSource.data.filter(user => user.id !== userId);
              this.dataSource = new MatTableDataSource<User>(newUsers);
              this.snackBar.open('User Deleted!', '', {
                duration: 2000,
              });
            }, err => {
              console.log(err);
            });
        }
      });
  }

}
