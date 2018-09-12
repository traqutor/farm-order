import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppMaterialModule } from './app-material.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { OrdersModule } from './orders/orders.module';
import { UsersModule } from './users/users.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './core/auth/auth.interceptor';
import { ConfirmDialogComponent } from './shared/dialogs/confirm-dialog/confirm-dialog.component';
import { ChangePasswordDialogComponent } from './shared/dialogs/change-password-dialog/change-password-dialog.component';
import { AlertDialogComponent } from './shared/dialogs/alert-dialog/alert-dialog.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppMaterialModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    CoreModule,
    OrdersModule,
    UsersModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  entryComponents: [
    ConfirmDialogComponent,
    ChangePasswordDialogComponent,
    AlertDialogComponent,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
