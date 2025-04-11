using Microsoft.AspNetCore.OutputCaching;

namespace CacheTest
{
    /// <summary>
    /// A policy to respect the no-cache and no-store directives of the Cache-Control request header
    /// </summary>
    public class CacheControlOutputCachePolicy : IOutputCachePolicy
    {
        /// <summary>
        /// Disallows cache lookup and storage following the no-cache and no-store directives in the Cache-Control request header
        /// </summary>
        /// <inheritdoc/>
        public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            if (context.HttpContext.Request.GetTypedHeaders().CacheControl?.NoCache == true)
            {
                context.AllowCacheLookup = false;
            }

            if (context.HttpContext.Request.GetTypedHeaders().CacheControl?.NoStore == true)
            {
                context.AllowCacheStorage = false;
            }

            return ValueTask.CompletedTask;
        }

        /// <inheritdoc/>
        public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc/>
        public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            return ValueTask.CompletedTask;
        }
    }
}
