import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatInput, MatOption, MatPaginator, MatSelect, MatTableDataSource } from '@angular/material';
import { OrdersService } from '../orders.service';
import { Order } from '../../shared/models/order';
import { DatePipe } from '@angular/common';
import { merge, Observable, of } from 'rxjs';
import { Farm } from '../../shared/models/farm';
import { SharedService } from '../../shared/shared.service';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit {

  dataSource: Order[] = [];
  displayedColumns = [
    { value: 'status', name: 'Status' },
    { value: 'orderChangeReason', name: 'Order change reason' },
    { value: 'deliveryDate', name: 'Delivery Date' },
    { value: 'tonsOrdered', name: 'Tons ordered' },
    { value: 'farm', name: 'Farm' },
  ];
  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;
  columnsToRender = ['status', 'orderChangeReason', 'deliveryDate', 'tonsOrdered', 'farm', 'settings'];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  dateFromValue;
  dateToValue;
  orderLength = 0;

  constructor(private ordersService: OrdersService,
              private datePipe: DatePipe,
              private sharedService: SharedService) {
  }

  ngOnInit() {
    this.farms$ = this.sharedService.getUserAssignedFarms();
    console.log(this.dateToValue);
    merge(this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          console.log(this.dateToValue);
          return this.ordersService!.getOrders({
            page: this.paginator.pageIndex,
            customers: [],
            farm: [],
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
          return of([]);
        })
      ).subscribe(data => this.dataSource = data);
  }

  filterByDate(dp1, dp2) {
    this.ordersService.getOrders({
      page: this.paginator.pageIndex,
      customers: [],
      farm: [],
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
        return of([]);
      })
    ).subscribe(data => this.dataSource = data);
  }

  filterByFarm(option: MatOption) {
    this.ordersService.getOrders({
      page: this.paginator.pageIndex,
      customers: [],
      statuses: [],
      farm: [option.value],
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
        return of([]);
      })
    ).subscribe(data => this.dataSource = data);
  }

  displayRow(row, column) {
    if (typeof row === 'object' && row !== null && row.name !== null) {
      return row.name;
    } else if (column.value === 'deliveryDate') {
      return this.datePipe.transform(row, 'yyyy-MM-dd');
    }
    return row;
  }

}
