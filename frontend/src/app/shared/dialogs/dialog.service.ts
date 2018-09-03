import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Observable } from 'rxjs';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { ChangePasswordDialogComponent } from './change-password-dialog/change-password-dialog.component';
import { AlertDialogComponent } from './alert-dialog/alert-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) {
  }

  public confirm(title: string, dialogMessage: string): Observable<boolean> {

    let dialogRef: MatDialogRef<ConfirmDialogComponent>;

    dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.dialogMessage = dialogMessage;

    return dialogRef.afterClosed();
  }

  public changePassword(type: string) {
    let dialogRef: MatDialogRef<ChangePasswordDialogComponent>;

    dialogRef = this.dialog.open(ChangePasswordDialogComponent, {
      width: '500px'
    });

    dialogRef.componentInstance.type = type;


    return dialogRef.afterClosed();
  }

  public alert(dialogMessage: string): Observable<boolean> {

    let dialogRef: MatDialogRef<AlertDialogComponent>;

    dialogRef = this.dialog.open(AlertDialogComponent);

    dialogRef.componentInstance.dialogMessage = dialogMessage;

    return dialogRef.afterClosed();
  }
}
