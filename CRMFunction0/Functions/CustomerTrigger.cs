
using CRMFunction0.Entities;
using CRMFunction0.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CRMFunction0.Functions
{
    public class CustomerTrigger
    {
        private readonly ILogger<CustomerTrigger> _logger;
        private readonly EmailService _emailService;

        public CustomerTrigger(
            ILogger<CustomerTrigger> logger,
            EmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [Function("CustomerTrigger")]
        public async Task Run(
            [CosmosDBTrigger(
                databaseName: "CRMDB",
                containerName: "Customers",
                Connection = "CosmosDb",
                LeaseContainerName = "leases",
                CreateLeaseContainerIfNotExists = true)]
            IReadOnlyList<Customer>? customers)
        {
            _logger.LogInformation("TRIGGER STARTED");

            if (customers is null || customers.Count == 0)
            {
                _logger.LogInformation("No customer changes detected.");
                return;
            }

            foreach (var customer in customers)
            {
                
                await _emailService.SendEmailAsync(customer);
            }
        }
    }
}