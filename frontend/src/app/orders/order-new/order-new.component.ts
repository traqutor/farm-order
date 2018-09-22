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


  ngOnInit() {
    this.farms$ = this.sharedService.getUserAssignedFarms(null);
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
      ]],
      ration: [null, [
        Validators.required
      ]],
      sheds: [null, [
        Validators.required
      ]],
      silos: [null, [
        Validators.required
      ]]
    });
  }

  getRations(farm: Farm) {
    this.rations$ = this.sharedService.getRations(farm.id);
  }

  getSheds(farm: Farm) {
    this.sheds$ = this.sharedService.getSheds(farm.id, null);
  }

  getSilos(sheds: Array<ISilo>) {
    this.silos$ = this.sharedService.getSilos(sheds, null);
  }

  onSubmit() {
    const {value, valid} = this.order;
    value.deliveryDate.toISOString();
    if (valid) {
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

  cancel() {
    this._location.back();
  }
}
