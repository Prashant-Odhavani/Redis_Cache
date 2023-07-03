namespace RedisDemo.IServices
{
    public interface IHyperLogLogDemo
    {
        string GetHyperLogLogLength(string Key);
        string AddHyperLogLog(string Key, string value);
    }
}
