import { Farm } from './farm';

export interface Order {
  id?: number;
  creationDate: string;
  modificationDate: string;
  status: Status;
  orderChangeReason: OrderChangeReason;
  deliveryDate: string;
  tonsOrdered: number;
  farm: Farm;
}

export interface Status {
  id?: number;
  name: string;
}

export interface OrderChangeReason {
  id?: number;
  name: string;
}
