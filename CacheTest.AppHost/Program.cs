var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CacheTest>("cachetest");

builder.Build().Run();
