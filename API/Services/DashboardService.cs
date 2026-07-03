using System.Globalization;
using API.Data.Contexts;
using API.DTOs.Dashboard;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using static API.Entities.Enums;

namespace API.Services;

public class DashboardService(AppDbContext context) : IDashboardService
{
    public async Task<DashboardDto> GetAsync()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var sixMonthsAgo = startOfMonth.AddMonths(-5);

        var activeContracts = await context.Contracts
            .CountAsync(c => c.Status == RentalStatus.Active);

        var newContractsThisMonth = await context.Contracts
            .CountAsync(c => c.CreatedAt >= startOfMonth);

        var monthlyIncome = await context.Payments
            .Where(p => p.TransactionType == TransactionType.Income && p.PaymentDate >= startOfMonth)
            .SumAsync(p => (decimal?)p.Amount) ?? 0m;

        var availableAssets = await context.Assets
            .CountAsync(a => a.Status == AssetStatus.Available);

        var rentedAssets = await context.Assets
            .CountAsync(a => a.Status == AssetStatus.Rented);

        var totalAssets = availableAssets + rentedAssets;

        // Outstanding balance across active/pending contracts
        var outstandingBalance = await context.Contracts
            .Where(c => c.Status == RentalStatus.Active || c.Status == RentalStatus.Pending)
            .Select(c => c.TotalAmount - (c.PaymentContracts
                .Where(pc => pc.Payment.TransactionType == TransactionType.Income && !pc.Payment.IsDeleted)
                .Sum(pc => (decimal?)pc.Payment.Amount) ?? 0m))
            .SumAsync(o => (decimal?)o) ?? 0m;

        // Overdue: Active contracts past endDate
        var overdueRaw = await context.Contracts
            .Where(c => c.Status == RentalStatus.Active && c.EndDate < now)
            .Select(c => new
            {
                c.Id,
                c.EndDate,
                CustomerName = c.Customer.Name,
                Outstanding = c.TotalAmount - (c.PaymentContracts
                    .Where(pc => pc.Payment.TransactionType == TransactionType.Income && !pc.Payment.IsDeleted)
                    .Sum(pc => (decimal?)pc.Payment.Amount) ?? 0m),
                AssetNames = c.ContractAssets.Select(ca => ca.Asset.Name).ToList()
            })
            .OrderBy(c => c.EndDate)
            .Take(15)
            .ToListAsync();

        var overdueContracts = overdueRaw
            .Where(x => x.Outstanding > 0)
            .Take(10)
            .Select(x => new OverdueContractDto
            {
                Id = x.Id,
                EndDate = x.EndDate,
                CustomerName = x.CustomerName,
                OutstandingBalance = x.Outstanding,
                AssetNames = x.AssetNames
            }).ToList();

        // Recent transactions
        var recentTransactions = await context.Payments
            .OrderByDescending(p => p.PaymentDate)
            .Take(8)
            .Select(p => new RecentTransactionDto
            {
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                TransactionType = p.TransactionType,
                PaymentMethod = p.PaymentMethod,
                CustomerName = p.PaymentContracts.Select(pc => pc.Contract.Customer.Name).FirstOrDefault(),
                Label = p.Description ?? p.Notes ?? p.PaymentContracts.Select(pc => pc.Contract.Customer.Name).FirstOrDefault()
            })
            .ToListAsync();

        // Monthly chart — fetch payments in window, group in memory
        var chartPayments = await context.Payments
            .Where(p => p.PaymentDate >= sixMonthsAgo)
            .Select(p => new { p.PaymentDate, p.Amount, p.TransactionType })
            .ToListAsync();

        var el = new CultureInfo("el-GR");
        var monthlyChart = Enumerable.Range(0, 6)
            .Select(i => startOfMonth.AddMonths(-5 + i))
            .Select(ms =>
            {
                var me = ms.AddMonths(1);
                var slice = chartPayments.Where(p => p.PaymentDate >= ms && p.PaymentDate < me).ToList();
                return new MonthlyChartDto
                {
                    Month = ms.ToString("MMM yy", el),
                    Income   = slice.Where(p => p.TransactionType == TransactionType.Income).Sum(p => p.Amount),
                    Expenses = slice.Where(p => p.TransactionType == TransactionType.Expense).Sum(p => p.Amount)
                };
            }).ToList();

        return new DashboardDto
        {
            Kpi = new KpiDto
            {
                ActiveContracts          = activeContracts,
                NewContractsThisMonth    = newContractsThisMonth,
                MonthlyIncome            = monthlyIncome,
                TotalOutstandingBalance  = outstandingBalance,
                AvailableAssets          = availableAssets,
                RentedAssets             = rentedAssets,
                TotalAssets              = totalAssets
            },
            OverdueContracts    = overdueContracts,
            RecentTransactions  = recentTransactions,
            MonthlyChart        = monthlyChart
        };
    }
}
