using Common.Extensions;
using Common.Filters;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddSecrets();
builder.Host.AddSerilog();

builder.Services.AddFilters();

builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Program).Assembly });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.AddSwagger("Weather Alerts API");

builder.Services.AddTransient<CommonExceptionFilter>();
   
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();