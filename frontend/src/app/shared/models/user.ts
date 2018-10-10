import { Customer } from './customer';
import { Role } from './role';
import { Farm } from './farm';

export interface User {
  id?: string;
  userName: string;
  customer: Customer;
  farms: [Farm];
  roleId: Role;
  role?: Role;
}
