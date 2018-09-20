import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedService } from '../../shared/shared.service';
import { OrdersService } from '../orders.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { Observable } from 'rxjs';
import { Farm } from '../../shared/models/farm';
import { OrderChangeReason, Status } from '../../shared/models/order';
import { DialogService } from '../../shared/dialogs/dialog.service';
import { Ration } from '../../shared/models/ration';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {

  constructor(private fb: FormBuilder,
              private sharedService: SharedService,
              private ordersService: OrdersService,
              private router: Router,
              private route: ActivatedRoute,
              private snackBar: MatSnackBar,
              private dialogService: DialogService) {
  }

  order: FormGroup;
  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;
  rations$: Observable<{ results: Array<Ration>, resultsCount: number }>;
  status$: Observable<{ results: Array<Status>, resultsCount: number }>;
  orderChangeReason$: Observable<{ results: Array<OrderChangeReason>, resultsCount: number }>;
  orderId;

  ngOnInit() {
    this.route
      .params
      .subscribe(params => {
        this.orderId = +params['id'];
        this.ordersService.getOrderById(this.orderId)
          .subscribe(order => {
            this.order = this.fb.group({
              tonsOrdered: [order.tonsOrdered, [
                Validators.required,
                Validators.min(1),
              ]],
              deliveryDate: [order.deliveryDate, [
                Validators.required
              ]],
              farm: [order.farm, [
                Validators.required
              ]],
              ration: [order.ration, [
                Validators.required
              ]],
              status: [order.status, [
                Validators.required
              ]],
              orderChangeReason: [order.orderChangeReason, [
                Validators.required
              ]]
            });
            this.getRations(order.farm);
          }, err => {
            this.dialogService.alert(err.error);
          });
      });
    this.farms$ = this.sharedService.getUserAssignedFarms();
    this.status$ = this.sharedService.getStatus();
    this.orderChangeReason$ = this.sharedService.getOrderChangeReason();
  }

  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  getRations(farm: Farm) {
    this.rations$ = this.sharedService.getRations(farm.id);
  }

  onSubmit() {
    const { value, valid } = this.order;
    console.log(value);
    if (valid) {
      this.ordersService.putOrder(this.orderId, value)
        .subscribe(() => {
          this.snackBar.open('Order Edited!', '', {
            duration: 2000,
          });
        }, err => {
          this.dialogService.alert(err.error);
        });
    }
  }
}
