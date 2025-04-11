
using Community.Microsoft.Extensions.Caching.PostgreSql;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Hybrid;

namespace CacheTest;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddDistributedPostgreSqlCache(options =>
        {
            options.ConnectionString = "Host=localhost;Database=postgres;Username=postgres;Password=postgres";
            options.SchemaName = "public";
            options.TableName = "Cache";
        });

        builder.Services.AddHybridCache();

        builder.Services.AddSingleton<IOutputCacheStore, HybridOutputCacheStore>();
        builder.Services.AddOutputCache(options =>
        {
            options.AddPolicy("CacheControl", builder => builder.AddPolicy<CacheControlOutputCachePolicy>().Expire(TimeSpan.FromMinutes(5)));
        });

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseOutputCache();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
