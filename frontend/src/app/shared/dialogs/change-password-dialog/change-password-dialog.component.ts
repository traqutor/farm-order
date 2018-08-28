import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PasswordValidation } from '../../validators/match-password.validator';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-change-password-dialog',
  templateUrl: './change-password-dialog.component.html',
  styleUrls: ['./change-password-dialog.component.css']
})
export class ChangePasswordDialogComponent implements OnInit {

  public type: string;
  form;

  constructor(private fb: FormBuilder,
              private dialogRef: MatDialogRef<ChangePasswordDialogComponent>) {
  }

  ngOnInit() {
    this.form = this.fb.group({
      oldPassword: [{ value: null, disabled: this.type === 'reset' }, [Validators.required, Validators.minLength(6)]],
      password: [null, [Validators.required, Validators.minLength(6)]],
      confirmPassword: [null, [Validators.required, Validators.minLength(6)]]
    }, { validator: PasswordValidation.MatchPassword });
  }

  submit() {
    const { value } = this.form;
    this.dialogRef.close(value);
  }

}
