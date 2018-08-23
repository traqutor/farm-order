import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSelect, MatTableDataSource } from '@angular/material';
import { OrdersService } from '../orders.service';
import { Order } from '../../shared/models/order';
import { DatePipe } from '@angular/common';
import { Observable, of } from 'rxjs';
import { Farm } from '../../shared/models/farm';
import { SharedService } from '../../shared/shared.service';
import { User } from '../../shared/models/user';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit {

  dataSource = new OrderDataSource([]);
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

  orderLength = 0;
  pageIndex = 0;

  constructor(private ordersService: OrdersService,
              private datePipe: DatePipe,
              private sharedService: SharedService) {
  }

  ngOnInit() {
    this.farms$ = this.sharedService.getUserAssignedFarms();
    this.searchOrders();
  }

  filterTableByFarm(option: MatSelect) {
    this.dataSource.filter = option.value.name;
  }

  filterByDate(dp1, dp2) {
    console.log(dp2);
    this.searchOrders({
      page: 0,
      customers: [],
      statuses: [],
      changeReasons: [],
      startDate: dp1 !== '' ? new Date(dp1).toISOString() : null,
      endDate: dp2 !== '' ? new Date(dp2).toISOString() : null,
      orderByAttribute: 0,
      sortOrder: 0
    });
  }

  changePage(event: MatPaginator) {
    this.pageIndex = event.pageIndex;
    this.searchOrders({
      page: event.pageIndex,
      customers: [],
      statuses: [],
      changeReasons: [],
      startDate: null,
      endDate: null,
      orderByAttribute: 0,
      sortOrder: 0
    });
  }

  searchOrders(searchParams = {
    page: 0,
    customers: [],
    statuses: [],
    changeReasons: [],
    startDate: null,
    endDate: null,
    orderByAttribute: 0,
    sortOrder: 0
  }) {
    this.ordersService.getOrders(searchParams).subscribe((res: { results: Array<Order>, resultsCount: number }) => {
      this.orderLength = res.resultsCount;
      this.dataSource = new OrderDataSource(res.results);
      this.dataSource.filterPredicate = (data: Order, filter: string) => {
        if (data.farm) {
          return data.farm.name.indexOf(filter) !== -1;
        }
      };
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

}


import { DataSource } from '@angular/cdk/collections';

// import 'rxjs/add/observable/of';

export class OrderDataSource extends DataSource<any> {
  filterPredicate: (data: Order, filter: string) => boolean;
  filter: any;
  constructor(private _orders: Order[]) {
    super();
  }

  connect(): Observable<Order[]> {
    return of(this._orders);
  }

  disconnect() {
  }
}
