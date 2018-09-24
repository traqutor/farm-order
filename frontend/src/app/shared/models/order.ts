import {Farm} from './farm';
import {Ration} from './ration';
import {ISilo} from "./silo";
import {IShed} from "./shed";

export interface IOrder {
  id?: number;
  creationDate?: string;
  modificationDate?: string;
  status?: Status;
  orderChangeReason?: OrderChangeReason;
  deliveryDate: string;
  tonsOrdered: number;
  farm: Farm;
  ration: Ration;
  sheds: Array<IShed>;
  silos: Array<ISilo>;
}

export interface Status {
  id?: number;
  name: string;
}

export interface OrderChangeReason {
  id?: number;
  name: string;
}
