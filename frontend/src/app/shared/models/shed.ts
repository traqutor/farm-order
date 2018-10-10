import {ISilo} from "./silo";

export interface IShed {
  id: number;
  name: string;
  silos: Array<ISilo>;
}
