import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Farm } from '../../shared/models/farm';
import { SharedService } from '../../shared/shared.service';

@Component({
  selector: 'app-order-new',
  templateUrl: './order-new.component.html',
  styleUrls: ['./order-new.component.css']
})
export class OrderNewComponent implements OnInit {

  constructor(private fb: FormBuilder,
              private sharedService: SharedService) {
  }

  order: FormGroup;
  farms$: Observable<{ results: Array<Farm>, resultCount: number }>;

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
    console.log(value);
  }

}
