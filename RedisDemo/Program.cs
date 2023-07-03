using RedisDemo.Data;
using RedisDemo.IServices;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
	options.InstanceName = "RedisDemo_";
});

builder.Services.AddSingleton(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");
    var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
    return connectionMultiplexer.GetDatabase();
});

builder.Services.AddScoped<IRateLimiter>(provider =>
{
    var database = provider.GetService<IDatabase>();
    return new RedisRateLimiter(database, 100, TimeSpan.FromMinutes(1));
});

builder.Services.AddScoped<IHyperLogLogDemo>(provider =>
{
    var database = provider.GetService<IDatabase>();
    return new HyperLogLogDemo(database);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
