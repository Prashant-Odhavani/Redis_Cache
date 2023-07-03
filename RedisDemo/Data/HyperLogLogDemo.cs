using RedisDemo.IServices;
using StackExchange.Redis;
namespace RedisDemo.Data
{
    public class HyperLogLogDemo : IHyperLogLogDemo
    {
        private readonly IDatabase _database;

        public HyperLogLogDemo(IDatabase database)
        {
            _database = database;
        }
        public string AddHyperLogLog(string Key, string value)
        {

            var isAdded = _database.HyperLogLogAdd(Key, value);
            return $"{value} added - {isAdded}";
        }

        public string GetHyperLogLogLength(string Key)
        {
           return _database.HyperLogLogLength(Key).ToString();
        }
    }
}
