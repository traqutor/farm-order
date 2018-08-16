import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersListComponent } from './users-list/users-list.component';
import { AppMaterialModule } from "../app-material.module";

@NgModule({
  imports: [
    CommonModule,
    AppMaterialModule,
  ],
  declarations: [UsersListComponent]
})
export class UsersModule { }
