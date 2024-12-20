import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FlexLayoutModule} from "@angular/flex-layout";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {HTTP_INTERCEPTORS} from '@angular/common/http';

import {AppRoutingModule} from './app-routing.module';
import {AppMaterialModule} from './app-material.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {SharedModule} from './shared/shared.module';
import {CoreModule} from './core/core.module';
import {OrdersModule} from './orders/orders.module';
import {UsersModule} from './users/users.module';
import {AuthInterceptor} from './core/auth/auth.interceptor';
import {ConfirmDialogComponent} from './shared/dialogs/confirm-dialog/confirm-dialog.component';
import {ChangePasswordDialogComponent} from './shared/dialogs/change-password-dialog/change-password-dialog.component';
import {AlertDialogComponent} from './shared/dialogs/alert-dialog/alert-dialog.component';
import {OverlayModule} from "@angular/cdk/overlay";
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { PasswordRecoveryComponent } from './password-recovery/password-recovery.component';
import { MultipleOrderDialogComponent } from './orders/multiple-order-dialog/multiple-order-dialog.component';

export const MY_FORMATS = {
  parse: {
    dateInput: 'DD/MM/YYYY',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'DD/MM/YYYY',
    monthYearA11yLabel: 'MMM YYYY',
  },
};


@NgModule({
  declarations: [
    AppComponent,
    PasswordResetComponent,
    PasswordRecoveryComponent,
    MultipleOrderDialogComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    OverlayModule,
    AppMaterialModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    CoreModule,
    OrdersModule,
    UsersModule,
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
  entryComponents: [
    ConfirmDialogComponent,
    ChangePasswordDialogComponent,
    AlertDialogComponent,
    MultipleOrderDialogComponent,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
