using System.Threading.Channels;
using API.Entities;

namespace API.Services;

public sealed class AuditChannel
{
    private readonly Channel<AuditLog> _channel = Channel.CreateBounded<AuditLog>(
        new BoundedChannelOptions(50_000) // μέγεθος buffer για τα audit logs που θα αποθηκευτούν στη βάση δεδομένων
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true
        });

    public ChannelWriter<AuditLog> Writer => _channel.Writer;
    public ChannelReader<AuditLog> Reader => _channel.Reader;
}
