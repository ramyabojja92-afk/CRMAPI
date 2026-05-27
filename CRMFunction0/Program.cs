using CRMFunction0.Services;
using CRMFunction0.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);



builder.ConfigureFunctionsWebApplication();



// dependency injection
builder.Services.AddScoped<EmailService>();



builder.Build().Run();

