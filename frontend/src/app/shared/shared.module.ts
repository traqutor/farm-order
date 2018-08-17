import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { LayoutModule } from '@angular/cdk/layout';
import { AppRoutingModule } from '../app-routing.module';
import { LoginComponent } from './components/login/login.component';
import { AppMaterialModule } from '../app-material.module';
import { ConfirmDialogComponent } from './dialogs/confirm-dialog/confirm-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AppRoutingModule,
    LayoutModule,
    AppMaterialModule
  ],
  providers: [],
  exports: [HeaderComponent],
  declarations: [HeaderComponent, LoginComponent, ConfirmDialogComponent],
})
export class SharedModule {
}
