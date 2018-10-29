import {Component, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {MatDialogRef} from "@angular/material";

import {IDateAmount, IMultipleOrder, ISiloWithMultipleAmount} from "../../shared/models/order";
import {Farm} from "../../shared/models/farm";
import {SharedService} from "../../shared/shared.service";
import {Ration} from "../../shared/models/ration";
import {IShed} from "../../shared/models/shed";
import index from "@angular/cli/lib/cli";


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
    let tmpDate = new Date();
    const control = (<FormArray>this.orderForm.controls['silos']).at(siloIndex).get('dateAmount') as FormArray;
    for (let i = 0; i < 4; i++) {
      let dDate: Date = new Date(tmpDate.getDate() + i);
      control.push(this.buildDateAmountFormGroup(dDate));
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
      id: [this.siloAmount.id],
      dateAmount: this.formBuilder.array([]),
    });
  }

  buildDateAmountFormGroup(date: Date): FormGroup {
    return this.formBuilder.group({
      date: [date],
      amount: [0],
    });
  }


  submit() {
    console.log('this.orderForm.value', this.orderForm.value);
    // this.dialogRef.close(this.orderForm.value);
  }

}
