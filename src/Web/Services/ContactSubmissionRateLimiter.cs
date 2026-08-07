using System.Threading.RateLimiting;

namespace Web.Services;

public sealed class ContactSubmissionRateLimiter : IDisposable
{
    private readonly PartitionedRateLimiter<string> _perIpLimiter =
        PartitionedRateLimiter.Create<string, string>(ipAddress =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: ipAddress,
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 3,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(30)
                }));

    private readonly FixedWindowRateLimiter _globalLimiter = new(new FixedWindowRateLimiterOptions
    {
        AutoReplenishment = true,
        PermitLimit = 20,
        QueueLimit = 0,
        Window = TimeSpan.FromHours(1)
    });

    public async ValueTask<ContactRateLimitResult> TryAcquireAsync(
        string ipAddress,
        CancellationToken cancellationToken)
    {
        using var ipLease = await _perIpLimiter.AcquireAsync(ipAddress, cancellationToken: cancellationToken);
        if (!ipLease.IsAcquired)
        {
            return ContactRateLimitResult.Rejected(GetRetryAfter(ipLease, TimeSpan.FromMinutes(30)));
        }

        using var globalLease = await _globalLimiter.AcquireAsync(cancellationToken: cancellationToken);
        return globalLease.IsAcquired
            ? ContactRateLimitResult.Allowed()
            : ContactRateLimitResult.Rejected(GetRetryAfter(globalLease, TimeSpan.FromHours(1)));
    }

    public void Dispose()
    {
        _perIpLimiter.Dispose();
        _globalLimiter.Dispose();
    }

    private static TimeSpan GetRetryAfter(RateLimitLease lease, TimeSpan fallback) =>
        lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
            ? retryAfter
            : fallback;
}

public readonly record struct ContactRateLimitResult(bool IsAllowed, TimeSpan RetryAfter)
{
    public static ContactRateLimitResult Allowed() => new(true, TimeSpan.Zero);
    public static ContactRateLimitResult Rejected(TimeSpan retryAfter) => new(false, retryAfter);
}
