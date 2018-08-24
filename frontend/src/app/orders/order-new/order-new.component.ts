import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Farm } from '../../shared/models/farm';
import { SharedService } from '../../shared/shared.service';
import { OrdersService } from '../orders.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-order-new',
  templateUrl: './order-new.component.html',
  styleUrls: ['./order-new.component.css']
})
export class OrderNewComponent implements OnInit {

  constructor(private fb: FormBuilder,
              private sharedService: SharedService,
              private ordersService: OrdersService,
              private router: Router,
              private route: ActivatedRoute,
              private snackBar: MatSnackBar ) {
  }

  order: FormGroup;
  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;

  ngOnInit() {
    this.farms$ = this.sharedService.getUserAssignedFarms();
    this.order = this.fb.group({
      tonsOrdered: [null, [
        Validators.required,
        Validators.min(1),
      ]],
      deliveryDate: [null, [
        Validators.required
      ]],
      farm: [null, [
        Validators.required
      ]]
    });
  }

  onSubmit() {
    const { value, valid } = this.order;
    value.deliveryDate.toISOString();
    if (valid) {
      this.ordersService.postOrder(value)
        .subscribe(() => {
          this.router.navigate(['../'], { relativeTo: this.route });
          this.snackBar.open('Order Created!', '', {
            duration: 2000,
          });
        }, err => {
          console.log(err);
        });
    }

  }

}
