using CRMAPI.Endpoints;
using CRMAPI.Interfaces;
using CRMAPI.Repos;
using CRMAPI.Endpoints;
using CRMAPI.Interfaces;
using CRMAPI.Repos;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Cosmos DB client
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var conn = config.GetConnectionString("CosmosDb");

    return new CosmosClient(conn);
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

// CORS
app.UseCors("AllowAll");

// endpoints
app.MapCustomerEndpoints();

app.Run();