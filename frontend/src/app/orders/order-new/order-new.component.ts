import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Observable} from 'rxjs';
import {ActivatedRoute, Router} from '@angular/router';
import {MatSnackBar} from '@angular/material';
import {Location} from '@angular/common';

import {Farm} from '../../shared/models/farm';
import {SharedService} from '../../shared/shared.service';
import {OrdersService} from '../orders.service';
import {DialogService} from '../../shared/dialogs/dialog.service';
import {Ration} from '../../shared/models/ration';
import {IShed} from "../../shared/models/shed";
import {ISilo} from "../../shared/models/silo";
import {IOrder} from "../../shared/models/order";

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
              private snackBar: MatSnackBar,
              private _location: Location,
              private dialogService: DialogService) {
  }

  order: FormGroup;
  farms$: Observable<{ results: Array<Farm>, resultsCount: number }>;
  rations$: Observable<{ results: [Ration], resultsCount: number }>;
  sheds$: Observable<{ results: [IShed], resultsCount: number }>;
  silos$: Observable<{ results: [ISilo], resultsCount: number }>;
  farms: Array<Farm> = [];
  orderTotalTonnage: number = 0;
  orderSilosTonnage: number = 0;
  orderTmp: IOrder = {
    id: null,
    farm: null,
    silos: [],
    sheds: [],
    creationDate: null,
    deliveryDate: null,
    modificationDate: null,
    orderChangeReason: null,
    ration: null,
    status: null,
    tonsOrdered: null
  };

  ngOnInit() {
    this.farms$ = this.sharedService.getUserAssignedFarms(null);
    this.farms$.subscribe((res: { results: Array<Farm>, resultsCount: number }) => {
      this.farms = res.results;
      this.order = this.fb.group({
        tonsOrdered: [0, [
          Validators.required,
          Validators.min(1),
        ]],
        deliveryDate: [null, [
          Validators.required
        ]],
        farm: [this.farms[0], [
          Validators.required
        ]],
        ration: [null, [
          Validators.required
        ]],
        sheds: [[], [
          Validators.required
        ]],
        silos: [[], [
          Validators.required
        ]]
      });
      this.getRations(this.farms[0]);
      this.getSheds(this.farms[0]);
    });
  }

  getTotalOrderAmount() {
    if (this.order) {
      this.orderTotalTonnage = this.order.controls.tonsOrdered.value;
    }
  }

  compare(val1, val2) {
    if (val1 && val2) {
      return val1.id === val2.id;
    }
  }

  getRations(farm: Farm) {
    this.rations$ = this.sharedService.getRations(farm.id);
  }

  getSheds(farm: Farm) {
    this.sheds$ = this.sharedService.getSheds(farm.id, null);
  }

  getSilos(sheds: Array<ISilo>) {

    this.orderSilosTonnage = 0;
    this.silos$ = this.sharedService.getSilos(sheds, null);
    this.silos$.subscribe((res: { results: Array<ISilo>, resultsCount: number }) => {

      let tmp: Array<ISilo> = [];

      console.log('this.orderTmp.silos', this.orderTmp.silos);

      this.orderTmp.silos.forEach((silo: ISilo) => {
        sheds.forEach((shed: IShed) => {
          if (shed.id === silo.shedId) {
            tmp.push(silo);
          }
        });
      });

      console.log('tmp', tmp);

      this.order.controls.silos.setValue(tmp);

      this.onSiloSelectChange();

    });
  }

  onSiloSelectChange() {

    setTimeout(() => {

      this.order.controls.silos.value.forEach((silo: ISilo) => {


        let addSilo = true;
        this.orderTmp.silos.forEach((siloTmp: ISilo) => {
          if (silo.id === siloTmp.id) {
            addSilo = false;
          }
        });
        if (addSilo) {
          this.orderTmp.silos.push(silo);
        }
      });

      this.orderTmp.silos.forEach((siloTmp: ISilo) => {
        this.order.controls.silos.value.forEach((silo: ISilo) => {
          if (silo.id === siloTmp.id) {
            silo.amount = siloTmp.amount;
          }
        });
      });
      this.recalculateOrderTonnage();

      console.log('this.orderTmp', this.orderTmp);

    }, 400);
  }

  onSiloAmountChange(silo: ISilo) {
    this.orderTmp.silos.forEach((siloTmp: ISilo) => {
      if (siloTmp.id === silo.id) {
        siloTmp.amount = silo.amount;
      }
    });
    this.recalculateOrderTonnage();
  }


  recalculateOrderTonnage() {
    this.orderSilosTonnage = 0;
    if (this.order) {
      this.order.controls.silos.value.forEach((silo: ISilo) => {
        this.orderSilosTonnage = this.orderSilosTonnage + silo.amount;
      });
    }
  }


  onSubmit() {

    const {value, valid} = this.order;
    value.deliveryDate.toISOString();

    if (valid) {

      if (this.orderTotalTonnage > this.orderSilosTonnage) {
        this.dialogService
          .confirm('Allocated amount is less then Total ordered tonnage', 'Are you sure you would like to proceed?')
          .subscribe(dialogRes => {
            if (dialogRes) {
              this.ordersService.postOrder(value)
                .subscribe(() => {
                  this.router.navigate(['../'], {relativeTo: this.route});
                  this.snackBar.open('Order Created!', '', {
                    duration: 2000,
                  });
                }, err => {
                  this.dialogService.alert(err.error);
                });
            }
          });
      }
      else {
        this.ordersService.postOrder(value)
          .subscribe(() => {
            this.router.navigate(['../'], {relativeTo: this.route});
            this.snackBar.open('Order Created!', '', {
              duration: 2000,
            });
          }, err => {
            this.dialogService.alert(err.error);
          });
      }

    }
  }

  cancel() {
    this._location.back();
  }
}
