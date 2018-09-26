import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {MatSnackBar} from '@angular/material';
import {Location} from "@angular/common";
import {Observable} from 'rxjs';

import {SharedService} from '../../shared/shared.service';
import {OrdersService} from '../orders.service';
import {Farm} from '../../shared/models/farm';
import {IOrder, OrderChangeReason, Status} from '../../shared/models/order';
import {DialogService} from '../../shared/dialogs/dialog.service';
import {Ration} from '../../shared/models/ration';
import {ISilo} from "../../shared/models/silo";
import {IShed} from "../../shared/models/shed";

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
              private _location: Location,
              private dialogService: DialogService) {
  }


  order: FormGroup;
  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;
  rations$: Observable<{ results: Array<Ration>, resultsCount: number }>;
  status$: Observable<{ results: Array<Status>, resultsCount: number }>;

  farms: { results: Array<Farm>, resultsCount: number };
  sheds$: Observable<{ results: [IShed], resultsCount: number }>;
  sheds: { results: [IShed], resultsCount: number };
  silos$: Observable<{ results: [ISilo], resultsCount: number }>;
  silos: { results: [ISilo], resultsCount: number };

  orderChangeReason$: Observable<{ results: Array<OrderChangeReason>, resultsCount: number }>;
  orderId;
  orderTmp: IOrder;
  orderTotalTonnage: number = 0;
  orderSilosTonnage: number = 0;

  ngOnInit() {
    this.route
      .params
      .subscribe(params => {
        this.orderId = +params['id'];
        this.ordersService.getOrderById(this.orderId)
          .subscribe(order => {
            this.orderTmp = order;
            this.orderTotalTonnage = order.tonsOrdered;

            this.order = this.fb.group({
              tonsOrdered: [this.orderTmp.tonsOrdered, [
                Validators.required,
                Validators.min(1),
              ]],
              deliveryDate: [this.orderTmp.deliveryDate, [
                Validators.required
              ]],
              farm: [this.orderTmp.farm, [
                Validators.required
              ]],
              ration: [this.orderTmp.ration, [
                Validators.required
              ]],
              sheds: [this.orderTmp.sheds, [
                Validators.required
              ]],
              silos: [this.orderTmp.silos, [
                Validators.required
              ]],
              status: [this.orderTmp.status, [
                Validators.required
              ]],
              orderChangeReason: [this.orderTmp.orderChangeReason, [
                Validators.required
              ]]
            });

            this.getRations(this.orderTmp.farm);
            this.getFarms();


            this.sheds$ = this.sharedService.getSheds(this.orderTmp.farm.id, null);
            this.sheds$.subscribe((res: { results: Array<IShed>, resultsCount: number }) => {
              let tmp: Array<IShed> = [];
              res.results.forEach((shed: IShed) => {
                this.orderTmp.silos.forEach((silo: ISilo) => {
                  if (shed.id === silo.shedId) {
                    tmp.push(shed);
                  }
                });
              });
              this.order.controls.sheds.setValue(tmp);
              this.silos$ = this.sharedService.getSilos(tmp, null);
              this.recalculateOrderTonnage();
            });


          }, err => {
            this.dialogService.alert(err.error);
          });
      });
    this.farms$ = this.sharedService.getUserAssignedFarms(null);
    this.status$ = this.sharedService.getStatus();
    this.orderChangeReason$ = this.sharedService.getOrderChangeReason();
  }

  getFarms() {
    this.sharedService.getUserAssignedFarms(null)
      .subscribe((farms: { results: Array<Farm>, resultsCount: number }) => {
        this.farms = farms;
      });

  }


  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  getTotalOrderAmount() {
    if (this.order) {
      this.orderTotalTonnage = this.order.controls.tonsOrdered.value;
    }
  }

  getRations(farm: Farm) {
    this.rations$ = this.sharedService.getRations(farm.id);
  }

  getSheds(farm: Farm) {
    this.sheds$ = this.sharedService.getSheds(farm.id, null);
  }

  recalculateOrderTonnage() {
    if (this.order) {
      this.orderSilosTonnage = 0;
      this.order.controls.silos.value.forEach((silo: ISilo) => {
        this.orderSilosTonnage = this.orderSilosTonnage + silo.amount;
      });
    }
  }

  onSiloSelectChange() {
    setTimeout(()=> {
      this.orderTmp.silos.forEach((silo: ISilo) => {
        this.order.controls.silos.value.forEach((siloTmp: ISilo) => {
          if (siloTmp.id === silo.id) {
            siloTmp.amount = silo.amount;
          }
        });
      });
      this.recalculateOrderTonnage();
    }, 300);
  }

  onSiloAmountChange(silo: ISilo) {
    this.orderTmp.silos.forEach((siloTmp: ISilo) => {
      if (siloTmp.id === silo.id) {
        siloTmp.amount = silo.amount;
      }
    });
    this.recalculateOrderTonnage();
  }

  getSilos(sheds: Array<IShed>) {

    this.orderSilosTonnage = 0;
    this.silos$ = this.sharedService.getSilos(sheds, null);
    this.silos$.subscribe((res: { results: Array<ISilo>, resultsCount: number }) => {

      let tmp: Array<ISilo> = [];

      if (this.order) {

        this.order.controls.silos.value.forEach((silo: ISilo) => {
          sheds.forEach((shed: IShed) => {
            if (shed.id === silo.shedId) {
              tmp.push(silo);
            }
          });
        });

        this.order.controls.silos.setValue(tmp);

        this.onSiloSelectChange();

      }

    });
  }

  onSubmit() {
    const {value, valid} = this.order;
    if (valid) {
      this.ordersService.putOrder(this.orderId, value)
        .subscribe(() => {
          this.snackBar.open('Order Changed!', '', {
            duration: 2000,
          });
        }, err => {
          this.dialogService.alert(err.error);
        });
    }
  }

  cancel() {
    this._location.back();
  }

}
