namespace RedisDemo.IServices
{
    public interface IRateLimiter
    {
        bool IsAllowed(string key, int limit);
        Task SetRateLimitAsync(int limit, TimeSpan duration, string key);
        Task<bool> ResetLimit(string key);
    }
}
