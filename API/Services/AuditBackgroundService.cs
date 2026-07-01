using API.Data.Contexts;
using API.Entities;

namespace API.Services;

public sealed class AuditBackgroundService(
    AuditChannel auditChannel,
    IServiceScopeFactory scopeFactory,
    ILogger<AuditBackgroundService> logger) : BackgroundService
{
    private const int BatchSize = 50; // μέγεθος batch για την αποθήκευση των audit logs στη βάση δεδομένων

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var batch = new List<AuditLog>(BatchSize);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!await auditChannel.Reader.WaitToReadAsync(stoppingToken)) break;

                batch.Clear();
                while (batch.Count < BatchSize && auditChannel.Reader.TryRead(out var log))
                    batch.Add(log);

                if (batch.Count == 0) continue;

                await using var scope = scopeFactory.CreateAsyncScope();
                var db = scope.ServiceProvider.GetRequiredService<AuditDbContext>();
                await db.AuditLogs.AddRangeAsync(batch, stoppingToken);
                await db.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                logger.LogError(ex, "AuditBackgroundService failed to persist batch.");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
