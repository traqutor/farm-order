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
import {IShedxSilo} from "../../shared/models/sheldxsilo";

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

  allFarmSheds: { results: IShed[], resultsCount: number };

  allShedsXSilos: Array<IShedxSilo> = [];
  orderShedsXSilos: Array<IShedxSilo> = [];

  tmpFarm: Farm = {id: null, name: null};

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

      this.farms = res ? res.results : [];

      if (this.farms.length > 0) {
        this.tmpFarm = this.farms[0];
      }

      this.order = this.fb.group({
        tonsOrdered: [0, [
          Validators.required,
          Validators.min(1),
        ]],
        deliveryDate: [null, [
          Validators.required
        ]],
        farm: [this.tmpFarm, [
          Validators.required
        ]],
        ration: [null, [
          Validators.required
        ]],
        silos: [[]]
      });

      if (this.tmpFarm.id !== null) {
        this.getRations(this.tmpFarm);
        this.getSheds(this.tmpFarm);

      }

    });

    // BSF 20181011 - Added these to provide entry for up to 10 silos
    this.addSilo();
    this.addSilo();
    this.addSilo();
    this.addSilo();
    this.addSilo();
    this.addSilo();
    this.addSilo();
    this.addSilo();
    this.addSilo();
    this.addSilo();
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


    });
  }

  onShedvalueChange(shed: IShedxSilo) {
    shed.silo = {id: null, shedId: null, name: null, amount: 0, capacity: 0};
  }

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

  removeSilo(index: number) {
    // BSF 20181011 - Modified to reset row rather than deleting
    // this.orderShedsXSilos.splice(index, 1);
    this.orderShedsXSilos[index].shed = {id: null, name: null, silos: []};
    this.orderShedsXSilos[index].silo = {id: null, shedId: null, name: null, amount: 0, capacity: 0};

    this.recalculateOrderTonnage();
  };


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

  submitDisabled() {
    let tmp = false;
    if (this.order.invalid || this.orderTotalTonnage < this.orderSilosTonnage || this.orderShedsXSilos.length <= 0) {
      tmp = true;
    }

    this.orderShedsXSilos.forEach((shedxsilo: IShedxSilo) => {
      // BSF 20181011 - Added logic to allow records to exist which have null shed / silo and an amount = 0
      if ((shedxsilo.silo.id === null || shedxsilo.shed.id === null) && shedxsilo.silo.amount > 0) {
        tmp = true;
      }
    });

    return tmp;
  }


  cancel() {
    this._location.back();
  }
}
