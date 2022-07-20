
using Polly;
using Polly.Caching;
using Polly.Registry;

namespace CacheLib
{
    public class CachePolicyFactory : ICachePolicyFactory
    {
        private const int UMMINUTO = 1;

        private readonly IPolicyRegistry<string> _policyRegistry;
        private readonly IAsyncCacheProvider _cacheProvider;

        public CachePolicyFactory(IAsyncCacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
            _policyRegistry = new PolicyRegistry();
        }

        public AsyncCachePolicy<TResult> CacheResponse<TResult>(string nome, TimeSpan expira) where TResult : HttpResponseMessage
        {
            Ttl cacheOnly200OKfilter(TResult result) => new Ttl(timeSpan: result.StatusCode == System.Net.HttpStatusCode.OK ? expira : TimeSpan.Zero);

            _policyRegistry.TryGet(nome, out AsyncCachePolicy<TResult> cachePolicy);

            if (cachePolicy is null)
            {
                cachePolicy =
                Policy.CacheAsync(_cacheProvider.AsyncFor<TResult>(), new ResultTtl<TResult>(cacheOnly200OKfilter));

                _policyRegistry.Add(nome, cachePolicy);
            }

            return cachePolicy;
        }

        public AsyncCachePolicy<TResult> Cache<TResult>(string nome, TimeSpan expira)
        {
            _policyRegistry.TryGet(nome, out AsyncCachePolicy<TResult> cachePolicy);

            if (cachePolicy is null)
            {
                cachePolicy = Policy.CacheAsync(_cacheProvider.AsyncFor<TResult>(), expira);

                _policyRegistry.Add(nome, cachePolicy);
            }

            return cachePolicy;
        }

        public AsyncCachePolicy<T> Cache<T>(string nome, int minutos = UMMINUTO)
        {
            _policyRegistry.TryGet(nome, out AsyncCachePolicy<T> cachePolicy);

            if (cachePolicy is null)
            {
                cachePolicy = Policy.CacheAsync<T>(_cacheProvider, TimeSpan.FromMinutes(minutos));
                _policyRegistry.Add(nome, cachePolicy);
            }

            return cachePolicy;
        }
    }
}
