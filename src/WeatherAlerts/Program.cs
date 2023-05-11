using Common.Extensions;
using Common.Filters;
using FluentValidation;
using Serilog;
using WeatherAlerts.Background;
using WeatherAlerts.Clients;
using WeatherAlerts.Services;
using WeatherAlerts.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddSecrets();
builder.Host.AddSerilog();

builder.Services.AddFilters();

builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.AddSwagger("Weather Alerts API");

builder.Services.AddTransient<CommonExceptionFilter>();
builder.Services.Configure<AlertsSettings>(builder.Configuration.GetSection("AlertSettings"));
builder.Services.AddHttpClient<IGisClient, GisClient>(opt =>
{
    opt.BaseAddress = new Uri("https://meteoinfo.ru");
});
builder.Services.AddScoped<IMeteoWarningsService, MeteoWarningsService>();
builder.Services.AddHostedService<MeteoWarningsUpdateService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();