using Polly.Caching;

namespace CacheLib
{
    public interface ICachePolicyFactory
    {
        AsyncCachePolicy<T> Cache<T>(string nome, int minutos = 1);
        public AsyncCachePolicy<TResult> CacheResponse<TResult>(string nome, TimeSpan expira) where TResult : HttpResponseMessage;
        public AsyncCachePolicy<TResult> Cache<TResult>(string nome, TimeSpan expira);
    }
}
