import {Component, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {MatDialogRef} from "@angular/material";

import {IDateAmount, IMultipleOrder, ISiloWithMultipleAmount} from "../../shared/models/order";
import {Farm} from "../../shared/models/farm";
import {SharedService} from "../../shared/shared.service";
import {Ration} from "../../shared/models/ration";
import {IShed} from "../../shared/models/shed";


@Component({
  selector: 'app-multiple-order-dialog',
  templateUrl: './multiple-order-dialog.component.html',
  styleUrls: ['./multiple-order-dialog.component.css']
})
export class MultipleOrderDialogComponent implements OnInit {

  multipleOrder: IMultipleOrder = {farm: null, ration: null, silos: [], notes: null};
  orderForm: FormGroup;
  siloAmount: ISiloWithMultipleAmount = {shed: null, id: null, dateAmount: []};
  farms: Array<Farm>;
  rations: Array<Ration>;
  allFarmSheds: Array<IShed>;
  orderSheds: Array<IShed> = [];

  constructor(public dialogRef: MatDialogRef<MultipleOrderDialogComponent>,
              private sharedService: SharedService,
              private formBuilder: FormBuilder) {
  }


  ngOnInit() {

    this.orderForm = this.formBuilder.group({
      farm: [this.multipleOrder.farm],
      ration: [this.multipleOrder.ration],
      silos: this.formBuilder.array([]),
      notes: [this.multipleOrder.notes],

    });
    this.addSiloAmount();
    this.getFarms();
  }

  get silos(): FormArray {
    return <FormArray>this.orderForm.get('silos');
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


  addSiloAmount(): void {
    for (let i = 0; i <= 9; i++) {
      this.silos.push(this.buildSilo());
      this.orderSheds.push({id: null, name: null, silos: null})
    }
  }

  shedSelected(shed: IShed, index: number) {
    console.log('index', index);
    this.orderSheds[index] = shed;
  }

  removeSiloAmount(i): void {
    this.silos.removeAt(i);
  }

  buildSilo(): FormGroup {
    let dateAmount: Array<IDateAmount> = [];
    let tmpDate = new Date();
    for (let i = 0; i < 4; i++) {
      dateAmount.push({date: tmpDate, amount: 0});
    }
    return this.formBuilder.group({
      shed: [this.siloAmount.shed],
      id: [this.siloAmount.id],
      dateAmount: this.buildDateAmount(),
    });
  }

  buildDateAmount(): Array<FormGroup> {
    let tmpArray: Array<FormGroup> = [];
    let tmpDate = new Date();
    for (let i = 0; i < 4; i++) {
      tmpArray.push(new FormGroup({date: new FormControl(tmpDate), amount: new FormControl(0) } ));
    }
    return tmpArray;
  }


  submit() {
    this.dialogRef.close(this.orderForm.value);
  }

}
