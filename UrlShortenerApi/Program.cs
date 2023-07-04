using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using UrlShortenerApi;
using AutoMapper;
using UrlShortenerApi.Profiles;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register MyDbContext with the dependency injection container
builder.Services.AddDbContext<MyDbContext>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "lg-cac-uni.redis.cache.windows.net:6380,password=INpJ8bAJRGIcN3jPt9BkRwPZJqmVINRD5AzCaPOBA9I=,ssl=True,abortConnect=False";
});
builder.Services.AddSingleton<RedisCache>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
