import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {MatDialog, MatPaginator, MatSelect, MatSnackBar} from '@angular/material';
import {DatePipe} from '@angular/common';
import {catchError, map, startWith, switchMap, throttle} from 'rxjs/operators';
import {interval, merge, Observable, of, Subscription} from 'rxjs';
import * as moment from 'moment';


import {OrdersService} from '../orders.service';
import {IMultipleOrder, IOrder} from '../../shared/models/order';
import {Farm} from '../../shared/models/farm';
import {SharedService} from '../../shared/shared.service';
import {AuthService} from '../../core/auth/auth.service';
import {DialogService} from '../../shared/dialogs/dialog.service';
import {User} from '../../shared/models/user';
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {MultipleOrderDialogComponent} from "../multiple-order-dialog/multiple-order-dialog.component";

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.css']
})
export class OrdersListComponent implements OnInit, OnDestroy {

  standardOrdersSource: IOrder[] = [];
  emergencyOrdersSource: IOrder[] = [];

  displayedColumns = [
    {value: 'status', name: 'Status', hideMobile: false},
    {value: 'orderChangeReason', name: 'Order change reason', hideMobile: false},
    {value: 'creationDate', name: 'Add Order Date', hideMobile: false},
    {value: 'modificationDate', name: 'Modification Date', hideMobile: true},
    {value: 'deliveryDate', name: 'Delivery Date', hideMobile: true},
    {value: 'tonsOrdered', name: 'Tons ordered', hideMobile: false},
    {value: 'ration', name: 'Ration', hideMobile: false},
    {value: 'farm', name: 'Farm', hideMobile: false}
  ];

  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;
  columnsToRender = ['status', 'creationDate', 'modificationDate', 'deliveryDate', 'orderChangeReason', 'tonsOrdered', 'ration', 'farm', 'settings'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatPaginator) emergencyPaginator: MatPaginator;
  @ViewChild('matSelect') matSelect: MatSelect;
  @ViewChild('matEmergencySelect') matEmergencySelect: MatSelect;

  dDate = new Date();

  dateFromValue: string;
  dateToValue: string;

  dateEmergencyFromValue: string;
  dateEmergencyToValue: string;

  orderLength = 0;
  user: User;
  subscriptions: Array<Subscription> = [];
  loading = false;

  constructor(private ordersService: OrdersService,
              private breakpointObserver: BreakpointObserver,
              private datePipe: DatePipe,
              private snackBar: MatSnackBar,
              private sharedService: SharedService,
              private authService: AuthService,
              private dialog: MatDialog,
              private dialogService: DialogService) {
  }

  ngOnInit() {

    this.setStartEndPeriodStandardDate();
    this.setStartEndPeriodEmergencyDate();

    this.loading = true;
    this.farms$ = this.sharedService.getUserAssignedFarms(null);
    this.user = this.authService.getUser();

    this.subscriptions.push(merge(this.paginator.page, interval(15000), this.matSelect.valueChange)
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
            orderByAttribute: 1,
            sortOrder: 1
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
      ).subscribe((data: Array<IOrder>) => {

          this.loading = false;

          const standardOrders: Array<IOrder> = [];
          const emergencyOrders: Array<IOrder> = [];


          for (const element of data) {
            if (element.isEmergency) {
              emergencyOrders.push(element);
            } else {
              standardOrders.push(element);
            }
          }


          this.standardOrdersSource = standardOrders;

          return this.standardOrdersSource;

        },
        err => {
          this.loading = false;
          this.dialogService.alert(err.error);
        }));

    this.subscriptions.push(merge(this.emergencyPaginator.page, interval(15000), this.matEmergencySelect.valueChange)
      .pipe(
        startWith({}),
        switchMap(() => {
          return this.ordersService.getOrders({
            page: this.emergencyPaginator.pageIndex,
            customers: [],
            farm: this.matEmergencySelect.value,
            statuses: [],
            changeReasons: [],
            startDate: this.dateEmergencyFromValue !== undefined && this.dateEmergencyFromValue !== null ? new Date(this.dateEmergencyFromValue).toISOString() : null,
            endDate: this.dateEmergencyToValue !== undefined && this.dateEmergencyToValue !== null ? new Date(this.dateEmergencyToValue).toISOString() : null,
            orderByAttribute: 1,
            sortOrder: 1
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
      ).subscribe((data: Array<IOrder>) => {

          this.loading = false;

          const standardOrders: Array<IOrder> = [];
          const emergencyOrders: Array<IOrder> = [];


          for (const element of data) {
            if (element.isEmergency) {
              emergencyOrders.push(element);
            } else {
              standardOrders.push(element);
            }
          }


          this.emergencyOrdersSource = emergencyOrders;

          return this.emergencyOrdersSource;

        },
        err => {
          this.loading = false;
          this.dialogService.alert(err.error);
        }));


  }

  setStartEndPeriodStandardDate() {

    let now = moment();

    let startDay = moment(now.weekday(1).hour(11).minute(15));
    let endDay = moment(now.weekday(4).hour(11).minute(15));

    if (moment().isBefore(startDay)) {
      console.log('before start Day');
      endDay = moment(now.weekday(1).hour(11).minute(15));
      startDay = moment(now.weekday(-3).hour(11).minute(15));
    } else {
      console.log('after start day');
      startDay = moment(now.weekday(1).hour(11).minute(15));
      endDay = moment(now.weekday(4).hour(11).minute(15));
    }

    console.log('now', now.toString());
    console.log('start Day', startDay.toString());
    console.log('end Day', endDay.toString());

    let from = moment(startDay);
    let to = moment();

    from.add(7, 'days');

    if (startDay.weekday() === 1) {
      to = moment(from);
      to.add(2, 'days');
    } else {
      to = moment(from);
      to.add(3, 'days');
    }

    console.log('from', from.toString());
    console.log('to', to.toString());

    this.dateFromValue = moment(from.hour(0).minute(0)).toISOString();
    this.dateToValue = moment(to.hour(0).minute(0)).toISOString();

  }


  setStartEndPeriodEmergencyDate() {

    let now = moment();

    let startDay = moment(now.weekday(1).hour(11).minute(15));
    let endDay = moment(now.weekday(4).hour(11).minute(15));


    if (moment().isBefore(startDay)) {
      console.log('before start Day');
      endDay = moment(now.weekday(1).hour(11).minute(15));
      startDay = moment(now.weekday(-3).hour(11).minute(15));
    } else {
      console.log('after start day');
      startDay = moment(now.weekday(1).hour(11).minute(15));
      endDay = moment(now.weekday(4).hour(11).minute(15));
    }


    console.log('start Day', startDay.toString());
    console.log('end Day', endDay.toString());

    let from = moment(endDay);
    let to = moment();

    if (startDay.weekday() === 1) {
      to = moment(from);
      to.add(3, 'days');
    } else {
      to = moment(from);
      to.add(2, 'days');
    }

    console.log('from', from.toString());
    console.log('to', to.toString());


    this.dateEmergencyFromValue = moment(from.hour(0).minute(0)).toISOString();
    this.dateEmergencyToValue = moment(to.hour(0).minute(0)).toISOString();
  }


  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Tablet)
    .pipe(
      map(result => result.matches)
    );


  filterByDate() {
    this.subscriptions.push(this.ordersService.getOrders({
      page: this.paginator.pageIndex,
      customers: [],
      farm: this.matSelect.value,
      statuses: [],
      changeReasons: [],
      startDate: this.dateFromValue !== undefined && this.dateFromValue !== null ? new Date(this.dateFromValue).toISOString() : null,
      endDate: this.dateToValue !== undefined && this.dateToValue !== null ? new Date(this.dateToValue).toISOString() : null,
      orderByAttribute: 1,
      sortOrder: 1
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

        const standardOrders: Array<IOrder> = [];
        const emergencyOrders: Array<IOrder> = [];

        for (const element of data) {

          if (element.isEmergency) {
            emergencyOrders.push(element);
          } else {
            standardOrders.push(element);
          }

        }

        this.standardOrdersSource = standardOrders;

        return this.standardOrdersSource;

      },
      err => {
        this.loading = false;
        this.dialogService.alert(err.error);
      }));
  }

  filterEmergencyByDate() {
    this.subscriptions.push(this.ordersService.getOrders({
      page: this.emergencyPaginator.pageIndex,
      customers: [],
      farm: this.matEmergencySelect.value,
      statuses: [],
      changeReasons: [],
      startDate: this.dateEmergencyFromValue !== undefined && this.dateEmergencyFromValue !== null ? new Date(this.dateEmergencyFromValue).toISOString() : null,
      endDate: this.dateEmergencyToValue !== undefined && this.dateEmergencyToValue !== null ? new Date(this.dateEmergencyToValue).toISOString() : null,
      orderByAttribute: 1,
      sortOrder: 1
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

        const standardOrders: Array<IOrder> = [];
        const emergencyOrders: Array<IOrder> = [];

        for (const element of data) {

          if (element.isEmergency) {
            emergencyOrders.push(element);
          } else {
            standardOrders.push(element);
          }

        }

        this.emergencyOrdersSource = emergencyOrders;

        console.log('this.emergencyOrdersSource', this.emergencyOrdersSource);

        return this.emergencyOrdersSource;

      },
      err => {
        this.loading = false;
        this.dialogService.alert(err.error);
      }));
  }


  displayRow(row, column): string {
    if (typeof row === 'object' && row !== null && row.name !== null) {
      if (column.value === 'silos') {
        return row[0].name;
      } else {
        return row.name;
      }
    } else if (column.value.indexOf('Date') !== -1) {
      return this.datePipe.transform(row, 'dd/MM/yyyy');
    }
    return row;
  }

  disableEdit(order: IOrder): boolean {
    let tmp: boolean;
    if (order && order.status && order.status.name === 'Confirmed' && this.user.role.name === 'Customer') {
      tmp = true;
    } else if (order && order.status && order.status.name === 'Delivered') {
      tmp = true;
    }
    return tmp;
  }

  deleteOrder(order: IOrder) {
    if (this.user.role.name === 'Admin' || this.user.role.name === 'CustomerAdmin') {
      this.dialogService
        .confirm('Delete order', 'Are you sure you would like to proceed?')
        .subscribe(dialogRes => {
          if (dialogRes)
            this.ordersService.deleteOrderById(order.id).subscribe(() => {
              this.filterByDate();
              this.snackBar.open('Order was deleted', '', {
                duration: 2500,
              });
            });
        });
    } else {
      this.snackBar.open('You have no rights to delete order', '', {
        duration: 2500,
      });
    }
  }

  orderProcess(isEmergency: boolean) {

    const dialogRef = this.dialog.open(MultipleOrderDialogComponent, {
      width: '80%',
      data: isEmergency,
      disableClose: true
    });

    dialogRef.afterClosed()
      .subscribe((resolvedOrder: IMultipleOrder) => {

        if (resolvedOrder) {
          this.filterByDate();
          this.filterEmergencyByDate();

        }

      });

  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub: Subscription) => {
      sub.unsubscribe();
    })
  }

}
