import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedService } from '../../shared/shared.service';
import { OrdersService } from '../orders.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { Observable } from 'rxjs';
import { Farm } from '../../shared/models/farm';

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
              private snackBar: MatSnackBar) {
  }

  order: FormGroup;
  farms$: Observable<{ results: Array<Farm>, resultCount: number }>;
  orderId;

  ngOnInit() {
    this.route
      .params
      .subscribe(params => {
        this.orderId = +params['id'];
        this.ordersService.getOrders({
          page: 0,
          customers: [],
          statuses: [],
          changeReasons: [],
          startDate: null,
          endDate: null,
          orderByAttribute: 0,
          sortOrder: 0
        }).subscribe(orders => {
          const oneOrder = orders.results.find(order => order.id === this.orderId);
          this.order = this.fb.group({
            tonsOrdered: [oneOrder.tonsOrdered, [
              Validators.required,
              Validators.min(1),
            ]],
            deliveryDate: [oneOrder.deliveryDate, [
              Validators.required
            ]],
            farm: [oneOrder.farm, [
              Validators.required
            ]]
          });
        });
      });
    this.farms$ = this.sharedService.getUserAssignedFarms();

  }

  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  onSubmit() {
    const { value, valid } = this.order;
    value.deliveryDate.toISOString();
    if (valid) {
      this.ordersService.postOrder(value)
        .subscribe(() => {
          this.router.navigate(['../'], { relativeTo: this.route });
          this.snackBar.open('User Created!', '', {
            duration: 2000,
          });
        }, err => {
          console.log(err);
        });
    }
  }
}
