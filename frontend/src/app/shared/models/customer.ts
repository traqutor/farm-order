import { CustomerSite } from './customer-site';

export interface Customer {
  id?: number;
  name: string;
  logo: string;
  cssFiePath: string;
  customerSites?: [CustomerSite];
  creationDate: string;
  modificationDate: string;
}
