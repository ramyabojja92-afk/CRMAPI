using CRMAPI.Entities;
using CRMAPI.Interfaces;
using Microsoft.Azure.Cosmos;

namespace CRMAPI.Repos
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly Container _container;

        public CustomerRepo(CosmosClient cosmosClient)
        {
            var database = cosmosClient
                .CreateDatabaseIfNotExistsAsync("CRMDB")
                .GetAwaiter()
                .GetResult();

            _container = database.Database
                .CreateContainerIfNotExistsAsync(
                    "Customers",
                    "/id"
                )
                .GetAwaiter()
                .GetResult()
                .Container;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Customer>(
                "SELECT * FROM c"
            );

            List<Customer> customers = new();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                customers.AddRange(response);
            }

            return customers;
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Customer>(
                    id,
                    new PartitionKey(id)
                );

                return response.Resource;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            customer.Id = Guid.NewGuid().ToString();

            var response = await _container.CreateItemAsync(
                customer,
                new PartitionKey(customer.Id)
            );

            return response.Resource;
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _container.UpsertItemAsync(
                customer,
                new PartitionKey(customer.Id)
            );
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<Customer>(
                id,
                new PartitionKey(id)
            );
        }
    }
}