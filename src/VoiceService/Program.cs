using System.Net.Http.Headers;
using Common.Base.Exceptions;
using Common.Extensions;
using Common.Filters;
using Common.Services.Exceptions;
using FluentValidation;
using HomeAssistantClient;
using Microsoft.Extensions.Options;
using Serilog;
using VoiceService;
using VoiceService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddSecrets();
builder.Host.AddSerilog();

builder.Services.AddTransient<CommonExceptionFilter>();
builder.Services.AddTransient<IExceptionErrorDataModelFactory, ExceptionErrorDataModelFactory>();
builder.Services.AddTransient<IErrorHttpResponseBuilder, ErrorHttpResponseBuilder>();

builder.Services.AddFilters();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.Configure<HomeAssistantSettings>(builder.Configuration.GetSection("HomeAssistant"));
builder.Services.Configure<AliceSettings>(builder.Configuration.GetSection("Alice"));

builder.Services.AddScoped<ITtsService, HomeAssistantTtsService>();

builder.Services.AddHttpClient<IHomeAssistantClient, HomeAssistantClient.HomeAssistantClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<HomeAssistantSettings>>().Value;
    client.BaseAddress = new Uri(settings.Host!);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
});

builder.AddSwagger("Voice Service API");

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();