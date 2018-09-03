import { Component, OnInit, Input, Output, forwardRef, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';


export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DatepickerComponent),
  multi: true
};

const noop = () => {
};

/** @title Basic datepicker */
@Component({
  selector: 'app-datepicker',
  templateUrl: 'datepicker.component.html',
  styleUrls: ['datepicker.component.css'],
  providers: [
    CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR
  ],
})

export class DatepickerComponent implements ControlValueAccessor {
  public mask = {
    guide: true,
    showMask: true,
    keepCharPositions: true,
    mask: [/\d/, /\d/, '/', /\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/]
  };
  innerValue: Date = new Date();
  private onTouchedCallback: () => void = noop;
  private onChangeCallback: (_: any) => void = noop;
  error = false;

  get value(): Date {
    return this.innerValue;
  }

  set value(v: Date) {
    if (v !== this.innerValue) {
      this.innerValue = v;
    }
  }

  writeValue(value: any): void {
    if (value !== this.innerValue) {
      this.innerValue = value;
    }
  }

  showRightDate() {
    try {
      return new Date(this.value);
    } catch (e) {
      console.log(e);
    }
  }

  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  onChange(event) {
    this.value = event;
    this.onBlur();
  }

  todate(value) {
    try {
      this.value = new Date(value);
    } catch (e) {
      console.log(e);
    }
  }

  onBlur() {
    this.onChangeCallback(this.innerValue);
  }
}


/**  Copyright 2018 Google Inc. All Rights Reserved.
 Use of this source code is governed by an MIT-style license that
 can be found in the LICENSE file at http://angular.io/license */
