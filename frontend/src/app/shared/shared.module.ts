import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { LayoutModule } from '@angular/cdk/layout';
import { AppRoutingModule } from '../app-routing.module';
import { LoginComponent } from './login/login.component';
import { AppMaterialModule } from '../app-material.module';

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
  declarations: [HeaderComponent, LoginComponent]
})
export class SharedModule {
}
