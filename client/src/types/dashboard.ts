export interface KpiDto {
  activeContracts: number;
  newContractsThisMonth: number;
  monthlyIncome: number;
  totalOutstandingBalance: number;
  availableAssets: number;
  rentedAssets: number;
  totalAssets: number;
}

export interface OverdueContractDto {
  id: string;
  customerName: string;
  outstandingBalance: number;
  endDate: string;
  assetNames: string[];
}

export interface RecentTransactionDto {
  id: string;
  amount: number;
  paymentDate: string;
  transactionType: 0 | 1; // 0=Income 1=Expense
  paymentMethod: 0 | 1 | 2;
  customerName?: string;
  label?: string;
}

export interface MonthlyChartDto {
  month: string;
  income: number;
  expenses: number;
}

export interface DashboardDto {
  kpi: KpiDto;
  overdueContracts: OverdueContractDto[];
  recentTransactions: RecentTransactionDto[];
  monthlyChart: MonthlyChartDto[];
}
