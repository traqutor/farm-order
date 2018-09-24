import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {MatPaginator, MatSelect} from '@angular/material';
import {DatePipe} from '@angular/common';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import {interval, merge, Observable, of} from 'rxjs';

import {OrdersService} from '../orders.service';
import {IOrder} from '../../shared/models/order';
import {Farm} from '../../shared/models/farm';
import {SharedService} from '../../shared/shared.service';
import {AuthService} from '../../core/auth/auth.service';
import {DialogService} from '../../shared/dialogs/dialog.service';
import {User} from '../../shared/models/user';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit, OnDestroy {

  dataSource: IOrder[] = [];
  displayedColumns = [
    {value: 'status', name: 'Status'},
    {value: 'orderChangeReason', name: 'Order change reason'},
    {value: 'deliveryDate', name: 'Delivery Date'},
    {value: 'tonsOrdered', name: 'Tons ordered'},
    {value: 'ration', name: 'Ration'},
    {value: 'farm', name: 'Farm'},
    {value: 'silos', name: 'Silos'},
  ];
  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;
  columnsToRender = ['status', 'orderChangeReason', 'deliveryDate', 'tonsOrdered', 'ration', 'farm', 'silos', 'settings'];
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

  displayRow(row, column): string {
    if (typeof row === 'object' && row !== null && row.name !== null) {
      if (column.value === 'silos') {
        return row[0].name;
      } else {
        return row.name;
      }
    } else if (column.value === 'deliveryDate') {
      return this.datePipe.transform(row, 'dd/MM/yyyy');
    }
    return row;
  }

  ngOnDestroy() {
    this.subscribe.unsubscribe();
  }

}
