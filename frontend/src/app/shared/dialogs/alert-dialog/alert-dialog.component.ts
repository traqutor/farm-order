import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-alert-dialog',
  templateUrl: './alert-dialog.component.html',
  styleUrls: ['./alert-dialog.component.css']
})
export class AlertDialogComponent implements OnInit {

  public dialogMessage: string;

  constructor(public dialogRef: MatDialogRef<AlertDialogComponent>) {
  }

  ngOnInit() {
  }

}
