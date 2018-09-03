import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { LayoutModule } from '@angular/cdk/layout';
import { AppRoutingModule } from '../app-routing.module';
import { LoginComponent } from './components/login/login.component';
import { AppMaterialModule } from '../app-material.module';
import { ConfirmDialogComponent } from './dialogs/confirm-dialog/confirm-dialog.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoginLayoutComponent } from './components/loginlayout/login-layout.component';
import { HomeLayoutComponent } from './components/home-layout/home-layout.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ChangePasswordDialogComponent } from './dialogs/change-password-dialog/change-password-dialog.component';
import { AlertDialogComponent } from './dialogs/alert-dialog/alert-dialog.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { TextMaskModule } from 'angular2-text-mask';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TextMaskModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    LayoutModule,
    AppMaterialModule
  ],
  providers: [],
  exports: [HeaderComponent, FooterComponent, DatepickerComponent],
  declarations: [
    HeaderComponent,
    LoginComponent,
    ConfirmDialogComponent,
    FooterComponent,
    LoginLayoutComponent,
    HomeLayoutComponent,
    ChangePasswordDialogComponent,
    AlertDialogComponent,
    DatepickerComponent
  ],
})
export class SharedModule {
}
