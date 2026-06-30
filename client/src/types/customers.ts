export interface CustomerDto {
  id: string;
  type?:number;
  name: string;
  afm: string;
  Dou?: string;
  phones: string[];
  email: string;
  address?: string;
  representative?: string;
  createdAt: string;
}

export interface CreateCustomerDto {
  name: string;
  afm: string;
  phones: string[];
  email: string;
  address?: string;
  city?: string;
  notes?: string;
}

export interface CustomerStatsDto {
  total: number;
  active: number;
  inactive: number;
  newThisMonth: number;
}

