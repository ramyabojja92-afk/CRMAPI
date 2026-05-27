using CRMFunction0.Entities;
using CRMFunction0.Entities;
using CRMFunction0.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CRMFunction0.Functions;

public class CosmosDBTrigger
{
    private readonly EmailService _emailService;
    private readonly ILogger<CosmosDBTrigger> _logger;

    public CosmosDBTrigger(
        EmailService emailService,
        ILogger<CosmosDBTrigger> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [Function("CosmosDBTrigger")]
    public void Run(
    [CosmosDBTrigger(
        databaseName: "CRMDB",
        containerName: "Customers",
        Connection = "CosmosDb",
        LeaseContainerName = "leases",
        CreateLeaseContainerIfNotExists = true)]
    IReadOnlyList<Customer> input)
    {
        _logger.LogInformation("TRIGGER FIRED!");
    }
}


    
