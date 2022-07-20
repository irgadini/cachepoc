using CacheLib;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();


//  Polly Cache Policy
builder.Services.AddScoped<Context>();
builder.Services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
builder.Services.AddSingleton<ICachePolicyFactory, CachePolicyFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
