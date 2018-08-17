import { Customer } from './customer';
import { Role } from './role';

export interface User {
  id?: string;
  customer: Customer;
  roleId: Role;
  role?: Role;
  userName: string;
}
