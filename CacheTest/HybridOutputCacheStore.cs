using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Hybrid;

namespace CacheTest
{
    /// <summary>
    /// A store for cached responses using <see cref="HybridCache"/>
    /// </summary>
    /// <param name="cache">The <see cref="HybridCache"/> instance</param>
    public class HybridOutputCacheStore(HybridCache cache) : IOutputCacheStore
    {
        /// <inheritdoc/>
        public ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            return cache.RemoveByTagAsync(tag, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            var options = new HybridCacheEntryOptions
            {
                Flags = HybridCacheEntryFlags.DisableUnderlyingData
            };

            var constrainedKey = ConstrainKey(key);

            return cache.GetOrCreateAsync(constrainedKey, _ => ValueTask.FromResult<byte[]?>(null), options, null, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            var options = new HybridCacheEntryOptions
            {
                Expiration = validFor,
                Flags = HybridCacheEntryFlags.DisableLocalCache
            };

            var constrainedKey = ConstrainKey(key);

            return cache.SetAsync(constrainedKey, value, options, tags, cancellationToken);
        }

        /// <summary>
        /// Maps non-printable characters to pipe characters to conform to validation in the HybridCache library.
        /// See the <see href="https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.Extensions.Caching.Hybrid/Internal/DefaultHybridCache.cs#L30">GitHub repository</see>.
        /// <br/> The pipe character was chosen as it is not a valid URL character.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The key with non-printable characters mapped to pipe characters</returns>
        private static string ConstrainKey(string key)
        {
            return new([.. key.Select(c => c < 32 ? '|' : c)]);
        }
    }
}
