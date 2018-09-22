import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatInput, MatOption, MatPaginator, MatSelect, MatTableDataSource } from '@angular/material';
import { OrdersService } from '../orders.service';
import { Order } from '../../shared/models/order';
import { DatePipe } from '@angular/common';
import { interval, merge, Observable, of } from 'rxjs';
import { Farm } from '../../shared/models/farm';
import { SharedService } from '../../shared/shared.service';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { AuthService } from '../../core/auth/auth.service';
import { DialogService } from '../../shared/dialogs/dialog.service';
import { User } from '../../shared/models/user';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit, OnDestroy {

  dataSource: Order[] = [];
  displayedColumns = [
    { value: 'status', name: 'Status' },
    { value: 'orderChangeReason', name: 'Order change reason' },
    { value: 'deliveryDate', name: 'Delivery Date' },
    { value: 'tonsOrdered', name: 'Tons ordered' },
    { value: 'ration', name: 'Ration' },
    { value: 'farm', name: 'Farm' },
  ];
  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;
  columnsToRender = ['status', 'orderChangeReason', 'deliveryDate', 'tonsOrdered', 'ration', 'farm', 'settings'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('matSelect') matSelect: MatSelect;

  dateFromValue: string;
  dateToValue: string;
  farmOption;
  orderLength = 0;
  user: User;
  subscribe;
  loading = false;

  constructor(private ordersService: OrdersService,
              private datePipe: DatePipe,
              private sharedService: SharedService,
              private authService: AuthService,
              private dialogService: DialogService) {
  }

  ngOnInit() {
    this.loading = true;
    this.farms$ = this.sharedService.getUserAssignedFarms(null);
    this.user = this.authService.getUser();
    this.subscribe = merge(this.paginator.page, interval(5000), this.matSelect.valueChange)
      .pipe(
        startWith({}),
        switchMap(() => {
          return this.ordersService.getOrders({
            page: this.paginator.pageIndex,
            customers: [],
            farm: this.matSelect.value,
            statuses: [],
            changeReasons: [],
            startDate: this.dateFromValue !== undefined && this.dateFromValue !== null ? new Date(this.dateFromValue).toISOString() : null,
            endDate: this.dateToValue !== undefined && this.dateToValue !== null ? new Date(this.dateToValue).toISOString() : null,
            orderByAttribute: 0,
            sortOrder: 0
          });
        }),
        map(data => {
          this.orderLength = data.resultsCount;
          return data.results;
        }),
        catchError(() => {
          this.loading = false;
          return of([]);
        })
      ).subscribe(data => {
          this.loading = false;
          return this.dataSource = data;
        },
        err => {
          this.loading = false;
          this.dialogService.alert(err.error);
        });
  }

  filterByDate() {
    this.ordersService.getOrders({
      page: this.paginator.pageIndex,
      customers: [],
      farm: this.farmOption,
      statuses: [],
      changeReasons: [],
      startDate: this.dateFromValue !== undefined && this.dateFromValue !== null ? new Date(this.dateFromValue).toISOString() : null,
      endDate: this.dateToValue !== undefined && this.dateToValue !== null ? new Date(this.dateToValue).toISOString() : null,
      orderByAttribute: 0,
      sortOrder: 0
    }).pipe(
      map(data => {
        this.orderLength = data.resultsCount;
        return data.results;
      }),
      catchError(() => {
        this.loading = false;
        return of([]);
      })
    ).subscribe(data => {
        this.loading = false;
        return this.dataSource = data;
      },
      err => {
        this.loading = false;
        this.dialogService.alert(err.error);
      });
  }

  displayRow(row, column) {
    if (typeof row === 'object' && row !== null && row.name !== null) {
      return row.name;
    } else if (column.value === 'deliveryDate') {
      return this.datePipe.transform(row, 'yyyy-MM-dd');
    }
    return row;
  }

  ngOnDestroy() {
    this.subscribe.unsubscribe();
  }

}
