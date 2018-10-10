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
import {IShedxSilo} from "../../shared/models/sheldxsilo";



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
  sheds$: Observable<{ results: IShed[], resultsCount: number }>;
  sheds: { results: IShed[], resultsCount: number };

  allFarmSheds: { results: IShed[], resultsCount: number };

  allShedsXSilos: Array<IShedxSilo> = [];
  orderShedsXSilos: Array<IShedxSilo> = [];

  silos$: Observable<{ results: ISilo[], resultsCount: number }>;
  silos: { results: ISilo[], resultsCount: number };

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

              // take all sheds an make the list shed_x_silo

              this.allFarmSheds = res;
              this.allFarmSheds.results.forEach((shed: IShed) => {
                shed.silos.forEach((silo: ISilo) => {
                  this.allShedsXSilos.push({shed, silo});
                });
              });

              // for each shed_x_silo add order amount

              this.orderTmp.silos.forEach((silo: ISilo) => {

                this.allShedsXSilos.forEach((sheadXSilo: IShedxSilo, index, object) => {

                  if (sheadXSilo.silo.id === silo.id && sheadXSilo.shed.id === silo.shedId) {

                    sheadXSilo.silo.amount = silo.amount;
                    this.orderShedsXSilos.push(sheadXSilo);
                    object.splice(index, 1);

                  }
                });
              });

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


  addSilo() {
    let shredxsilo: IShedxSilo = {
      shed: {id: null, silos: [], name: null},
      silo: {id: null, shedId: null, name: null, amount: 0, capacity: 0}
    };
    if (this.orderShedsXSilos.length <= 9) {
      this.orderShedsXSilos.push(shredxsilo);
    } else {
      this.snackBar.open('The limit of silos is 10 per order!', '', {
        duration: 2500,
      });

    }
    this.recalculateOrderTonnage();
  }

  removeSilo(index: number) {
    this.orderShedsXSilos.splice(index, 1);
    this.recalculateOrderTonnage();
  };

  onSiloSelectChange(shedxsilo: IShedxSilo, index: number) {

    setTimeout(() => {
      this.orderShedsXSilos.forEach((shedxsiloTmp: IShedxSilo, indexTmp) => {

        if (shedxsilo.shed.id === shedxsiloTmp.shed.id && shedxsilo.silo.id === shedxsiloTmp.silo.id && indexTmp !== index) {
          shedxsilo.silo = {id: null, shedId: null, name: null, amount: 0, capacity: 0};
          this.snackBar.open('There is such silos selected!', '', {
            duration: 2500,
          });
          return;
        }
      });

      this.recalculateOrderTonnage();
    }, 200);

  }


  recalculateOrderTonnage() {

    setTimeout(() => {
      this.orderSilosTonnage = 0;
      this.orderShedsXSilos.forEach((shedxsilo: IShedxSilo) => {
        this.orderSilosTonnage = this.orderSilosTonnage + shedxsilo.silo.amount;

      }, 200);
    });

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

  onShedvalueChange(shed: IShedxSilo) {
    shed.silo = {id: null, shedId: null, name: null, amount: 0, capacity: 0};
  }

  onSubmit() {
    const tmpSilos: Array<ISilo> = [];
    tmpSilos.length = 0;
    this.orderShedsXSilos.forEach((shedxsilo: IShedxSilo) => {
      if (shedxsilo.silo.id !== null) {
        tmpSilos.push(shedxsilo.silo);
      }
    });

    this.order.controls.silos.setValue(tmpSilos);

    const {value, valid} = this.order;

    if (valid) {

      if (this.orderTotalTonnage > this.orderSilosTonnage) {

        this.dialogService
          .confirm('Allocated amount is less then Total ordered tonnage', 'Are you sure you would like to proceed?')
          .subscribe(dialogRes => {
            if (dialogRes) {
              this.ordersService.putOrder(this.orderId, value)
                .subscribe(() => {
                  this.snackBar.open('Order Changed!', '', {
                    duration: 2000,
                  });
                }, err => {
                  this.dialogService.alert(err.error);
                });
            }
          });

      } else {

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

  }

  submitDisabled() {
    let tmp = false;
    if (this.order.invalid || this.orderTotalTonnage < this.orderSilosTonnage || this.orderShedsXSilos.length <= 0) {
      tmp = true;
    }

    this.orderShedsXSilos.forEach( (shedxsilo:IShedxSilo) => {
      if (shedxsilo.silo.id === null || shedxsilo.shed.id === null ) {
        tmp = true;
      }
    });

    return tmp;
  }

  cancel() {
    this._location.back();
  }

}
