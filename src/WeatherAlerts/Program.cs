using Common.Extensions;
using Common.Filters;
using Common.Influx;
using FluentValidation;
using InfluxDB.Client.Api.Domain;
using Serilog;
using WeatherAlerts;
using WeatherAlerts.Background;
using WeatherAlerts.Clients;
using WeatherAlerts.Services;
using WeatherAlerts.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddSecrets();
builder.Host.AddSerilog();

builder.Services.AddFilters();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.AddSwagger("Weather Alerts API");

builder.Services.AddTransient<CommonExceptionFilter>();
builder.Services.Configure<AlertsSettings>(builder.Configuration.GetSection("AlertSettings"));
builder.Services.AddHttpClient<IGisClient, GisClient>(opt =>
{
    opt.BaseAddress = new Uri(GisApi.BaseUrl);
});
builder.Services.AddSingleton<IMeteoWarningMerger, MeteoWarningMerger>();
builder.Services.AddScoped<IMeteoWarningsService, MeteoWarningsService>();
builder.Services.AddScoped<IMeteoWarningsDumper, MeteoWarningsDumper>();
builder.Services.AddHostedService<MeteoWarningsUpdateService>();

builder.Services.Configure<InfluxSettings>(builder.Configuration.GetSection("Influx"));
builder.Services.AddScoped<IInfluxManagementClient, InfluxManagementClient>();
builder.Services.AddScoped<IInfluxClient, InfluxClient>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var influx = services.GetRequiredService<IInfluxManagementClient>();
        await influx.EnsureBucketCreated(InfluxBuckets.MainBucketName, 
            new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, (long)TimeSpan.FromDays(30).TotalSeconds));
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating the database");
        throw;
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();