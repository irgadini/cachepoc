namespace CacheLib
{
    public interface ICacheable
    {
        public string Key { get; }
        public TimeSpan Expire { get; }
    }
}
