import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { User } from '../../shared/models/user';
import { OrdersService } from '../orders.service';
import { Order } from '../../shared/models/order';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit {

  dataSource = new MatTableDataSource<Order>([]);
  displayedColumns = [
    { value: 'status', name: 'Status' },
    { value: 'orderChangeReason', name: 'Order change reason' },
    { value: 'deliveryDate', name: 'Delivery Date' },
    { value: 'tonsOrdered', name: 'Tons ordered' },
    { value: 'farm', name: 'Farm' },
  ];
  columnsToRender = ['status', 'orderChangeReason', 'deliveryDate', 'tonsOrdered', 'farm', 'settings'];

  constructor(private ordersService: OrdersService,
              private datePipe: DatePipe) {
  }

  ngOnInit() {
    this.ordersService.getOrders({
      page: 0,
      customers: [],
      statuses: [],
      changeReasons: [],
      startDate: null,
      endDate: null,
      orderByAttribute: 0,
      sortOrder: 0
    }).subscribe((res: { results: Array<Order>, resultCount: number }) => {
      this.dataSource = new MatTableDataSource<Order>(res.results);
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
