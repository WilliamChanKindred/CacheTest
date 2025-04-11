using Microsoft.AspNetCore.OutputCaching;

namespace CacheTest
{
    public class CacheControlOutputCachePolicy : IOutputCachePolicy
    {
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

        public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            return ValueTask.CompletedTask;
        }
    }
}
