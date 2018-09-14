import { Farm } from './farm';
import { Ration } from './ration';

export interface Order {
  id?: number;
  creationDate?: string;
  modificationDate?: string;
  status?: Status;
  orderChangeReason?: OrderChangeReason;
  deliveryDate: string;
  tonsOrdered: number;
  farm: Farm;
  ration: Ration;
}

export interface Status {
  id?: number;
  name: string;
}

export interface OrderChangeReason {
  id?: number;
  name: string;
}
