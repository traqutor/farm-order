import {Component, Inject, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormGroup} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material";


import {IMultipleOrder, ISiloWithMultipleAmount} from "../../shared/models/order";
import {Farm} from "../../shared/models/farm";
import {SharedService} from "../../shared/shared.service";
import {Ration} from "../../shared/models/ration";
import {IShed} from "../../shared/models/shed";
import {OrdersService} from "../orders.service";
import * as moment from "moment";


@Component({
  selector: 'app-multiple-order-dialog',
  templateUrl: './multiple-order-dialog.component.html',
  styleUrls: ['./multiple-order-dialog.component.css']
})
export class MultipleOrderDialogComponent implements OnInit {

  multipleOrder: IMultipleOrder = {farm: null, ration: null, silos: [], notes: null, isEmergency: false};
  orderForm: FormGroup;
  siloAmount: ISiloWithMultipleAmount = {shed: null, id: null, silo: null, dateAmount: []};
  farms: Array<Farm>;
  rations: Array<Ration>;
  allFarmSheds: Array<IShed>;
  orderSheds: Array<IShed> = [];
  startOrderDate;


  errorMessage: string;

  constructor(public dialogRef: MatDialogRef<MultipleOrderDialogComponent>,
              private sharedService: SharedService,
              private orderService: OrdersService,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public isEmergency: boolean) {
  }


  ngOnInit() {

    if (this.isEmergency) {
      this.setStartEndPeriodEmergencyDate();
    } else {
      this.setStartEndPeriodStandardDate();
    }

    this.orderForm = this.formBuilder.group({
      farm: [this.multipleOrder.farm],
      ration: [this.multipleOrder.ration],
      silos: this.formBuilder.array([]),
      notes: [this.multipleOrder.notes],
      isEmergency: [this.isEmergency],
    });
    this.addSilosAmountRows();
    this.getFarms();
  }

  get silos(): FormArray {
    return <FormArray>this.orderForm.get('silos');
  }

  dateAmounts(index: number): FormArray {
    return (<FormArray>this.orderForm.controls['silos']).at(index).get('dateAmount') as FormArray;
  }


  getFarms() {
    this.sharedService.getUserAssignedFarms(null)
      .subscribe((farms: { results: Array<Farm>, resultsCount: number }) => {
        this.farms = farms.results;
      });

  }

  getRations(farm: Farm) {
    this.sharedService.getRations(farm.id).subscribe((res: { results: [Ration], resultsCount: number }) => {
      this.rations = res.results;
    });
  }

  getSheds(farm: Farm) {
    this.sharedService.getSheds(farm.id, null).subscribe((res: { results: [IShed], resultsCount: number }) => {
      this.allFarmSheds = res.results;
    });
  }


  addSilosAmountRows(): void {
    for (let i = 0; i <= 9; i++) {
      this.silos.push(this.buildSiloFormGroup());
      this.addSiloAmountDateRow(i);
      this.orderSheds.push({id: null, name: null, silos: null})
    }
  }

  addSiloAmountDateRow(siloIndex: number) {
    let tmpDate = new Date(this.startOrderDate);
    const control = (<FormArray>this.orderForm.controls['silos']).at(siloIndex).get('dateAmount') as FormArray;
    for (let i = 0; i < 4; i++) {
      control.push(this.buildDateAmountFormGroup(tmpDate));
      tmpDate.setDate(tmpDate.getDate() + 1);
    }
  }

  shedSelected(shed: IShed, index: number) {
    this.orderSheds[index] = shed;
  }

  removeSiloAmount(i): void {
    this.silos.removeAt(i);
  }

  buildSiloFormGroup(): FormGroup {
    return this.formBuilder.group({
      shed: [this.siloAmount.shed],
      silo: [this.siloAmount.silo],
      dateAmount: this.formBuilder.array([]),
    });
  }

  buildDateAmountFormGroup(date: Date): FormGroup {
    const dDate: Date = new Date(date);
    return this.formBuilder.group({
      date: [dDate],
      amount: [0],
    });
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

    this.startOrderDate = from;

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

    this.startOrderDate = from;

  }


  submit() {
    this.errorMessage = null;

    const tmpOrder: IMultipleOrder = {
      farm: this.orderForm.value.farm,
      isEmergency: this.orderForm.value.isEmergency,
      notes: this.orderForm.value.notes,
      ration: this.orderForm.value.ration,
      silos: []
    };

    // clean up the object form from null values
    this.orderForm.value.silos.forEach((sil: ISiloWithMultipleAmount) => {
      if (sil.silo && sil.silo.id) {
        let tmpSilo: ISiloWithMultipleAmount = {id: sil.silo.id, dateAmount: sil.dateAmount};
        tmpOrder.silos.push(tmpSilo);
      }
    });


    this.orderService.putMultipleOrder(tmpOrder).subscribe(() => {
      this.dialogRef.close(this.orderForm.value);
    }, error => {
      this.errorMessage = JSON.stringify(error);
    });
  }

}
