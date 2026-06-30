export interface CustomerDto {
   id: string;
  name: string;
  afm: string;
  dou?: string;
  address?: string;
  representative?: string;
  isActive: boolean;
  createdAt: string;
  contacts: ContactDto[];
}

export interface CreateCustomerDto {
  name: string;
  afm: string;
  phones: string[];
  dou?: string;
  representative?: string;
}

export interface ContactDto {
  id: string;
  name: string;
  phone?: string;
  email?: string;
  canUseAsset: boolean;
  notes?: string;
}

export interface CustomerStatsDto {
  total: number;
  active: number;
  inactive: number;
  newThisMonth: number;
}

