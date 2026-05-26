using CRMAPI.Endpoints;
using CRMAPI.Interfaces;
using CRMAPI.Repos;
using CRMAPI.Endpoints;
using System.Net.Http;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Cosmos DB client
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var conn = config.GetConnectionString("CosmosDb");

    var options = new CosmosClientOptions
    {
        ConnectionMode = ConnectionMode.Gateway,

        LimitToEndpoint = true,

        HttpClientFactory = () =>
        {
            HttpMessageHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            return new HttpClient(handler);
        }
    };

    return new CosmosClient(conn, options);
});

// Repo
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();




var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// endpoints
app.MapCustomerEndpoints();

app.Run();