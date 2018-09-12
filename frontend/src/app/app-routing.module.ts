import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './shared/components/login/login.component';
import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { UsersListComponent } from './users/users-list/users-list.component';
import { AuthGuard } from './core/auth/auth.guard';
import { UserNewComponent } from './users/user-new/user-new.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { OrderNewComponent } from './orders/order-new/order-new.component';
import { OrderEditComponent } from './orders/order-edit/order-edit.component';
import { LoginLayoutComponent } from './shared/components/loginlayout/login-layout.component';
import { HomeLayoutComponent } from './shared/components/home-layout/home-layout.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', data: { title: 'First Component' }, pathMatch: 'full' },
  {
    path: 'login', component: LoginLayoutComponent,
    children: [
      { path: '', component: LoginComponent }
    ]
  },
  {
    path: 'main', component: HomeLayoutComponent,
    children: [
      { path: '', redirectTo: 'orders', pathMatch: 'full' },
      { path: 'orders', component: OrdersListComponent, canActivate: [AuthGuard] },
      { path: 'orders/new', component: OrderNewComponent, canActivate: [AuthGuard] },
      { path: 'orders/:id/edit', component: OrderEditComponent, canActivate: [AuthGuard] },
      { path: 'users', component: UsersListComponent, canActivate: [AuthGuard] },
      { path: 'users/new', component: UserNewComponent, canActivate: [AuthGuard] },
      { path: 'users/:id/edit', component: UserEditComponent, canActivate: [AuthGuard] },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
