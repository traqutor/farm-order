import {Component, OnInit} from '@angular/core';
import {UsersService} from '../users.service';
import {SharedService} from '../../shared/shared.service';
import {Observable} from 'rxjs';
import {User} from '../../shared/models/user';
import {MatSelect, MatSnackBar, MatTableDataSource} from '@angular/material';
import {DialogService} from '../../shared/dialogs/dialog.service';
import {Customer} from '../../shared/models/customer';
import {AuthService} from '../../core/auth/auth.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {

  customers$: Observable<{ results: Array<Customer>, resultCount: number }>;

  dataSource = new MatTableDataSource<User>([]);

  displayedColumns = [
    {value: 'userName', name: 'User Name'},
    {value: 'customer', name: 'Customer'},
    {value: 'role', name: 'Role'},
    {value: 'entityStatus', name: 'Status'},
  ];
  columnsToRender = ['userName', 'customer', 'role', 'entityStatus', 'settings'];
  user;
  loading = false;

  constructor(private usersService: UsersService,
              private authService: AuthService,
              private sharedService: SharedService,
              private dialogService: DialogService,
              private snackBar: MatSnackBar) {
  }

  ngOnInit() {
    this.loading = true;
    const user = this.authService.getUser();
    if (user.role.name !== 'Admin') {
      const newDisplayedColumns = this.displayedColumns.filter(column => column.value !== 'customer');
      const newColumnsToRender = this.columnsToRender.filter(column => column !== 'customer');
      this.displayedColumns = newDisplayedColumns;
      this.columnsToRender = newColumnsToRender;
    }
    this.usersService.getUsers()
      .subscribe((users: { results: [User], resultCount: number }) => {
        this.loading = false;
        this.dataSource = new MatTableDataSource<User>(users.results);
        this.dataSource.filterPredicate = (data: User, filter: string) => {
          if (data.customer) {
            return data.customer.name.indexOf(filter) !== -1;
          }
        };
      }, err => {
        this.dialogService.alert(err.error);
      });
    this.customers$ = this.sharedService.getCustomers();
  }

  displayRow(row, column) {

    if (typeof row === 'object' && row !== null && row.name !== null) {
      return row.name;
    } else {
      if (column.value === 'entityStatus') {
        return row === 0 ? 'Active' : 'Deleted';
      } else {
        return row;
      }
    }
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
              this.dialogService.alert(err.error);
            });
        }
      });
  }

  activateUser(user: User, userId: string) {
    this.dialogService
      .confirm('Confirm Action', 'Are you sure you wanna Activate User')
      .subscribe(dialogRes => {
        if (dialogRes) {
          user.entityStatus = 0;
          console.log('user', user);
          this.usersService.putUser(user, userId)
            .subscribe(() => {
              const newUsers = this.dataSource.data.filter(user => user.id !== userId);
              this.dataSource = new MatTableDataSource<User>(newUsers);
              this.snackBar.open('User Activated!', '', {
                duration: 2000,
              });
            }, err => {
              this.dialogService.alert(err.error);
            });
        }
      });



  }

}
