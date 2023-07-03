using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RedisDemo.IServices;
using StackExchange.Redis;
using System.Threading.RateLimiting;
namespace RedisDemo.Data
{
    public class RedisRateLimiter : IRateLimiter
    {
        private readonly IDatabase _database;
        private readonly int _limit;
        private readonly TimeSpan _interval;

        public RedisRateLimiter(IDatabase database, int limit, TimeSpan interval)
        {
            _database = database;
            _limit = limit;
            _interval = interval;
        }

        public bool IsAllowed(string key, int limit)
        {
            var count = _database.StringIncrement(key);
            if (count == 1)
            {
                _database.KeyExpire(key, _interval);
            }
            return count <= limit;
        }

        public async Task SetRateLimitAsync(int limit, TimeSpan duration, string key)
        {
            // Store the rate limit values in Redis using a hash
            var hashEntries = new HashEntry[]
            {
            new HashEntry("limit", limit),
            new HashEntry("duration", (int)duration.TotalSeconds)
            };
            await _database.HashSetAsync("ratelimit:settings", hashEntries);

            await _database.StringSetAsync(key, "0", duration);
        }

        public async Task<bool> ResetLimit(string key)
        {
           return await _database.StringSetAsync(key, "0");
        }
       
    }
}
