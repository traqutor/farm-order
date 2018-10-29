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
  notes: string;
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

export interface IMultipleOrder {
  ration: Ration;
  farm: Farm;
  silos: Array<ISiloWithMultipleAmount>
  notes: string;
}

export interface ISiloWithMultipleAmount {
  shed: IShed;
  id: number;
  dateAmount: Array<IDateAmount>
}

export interface IDateAmount {
  amount: number,
  date: Date
}
