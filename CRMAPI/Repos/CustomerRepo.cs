using CRMAPI.Entities;
using CRMAPI.Interfaces;
using CRMAPI.Entities;
using CRMAPI.Interfaces;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using Container = Microsoft.Azure.Cosmos.Container;

namespace CRMAPI.Repos
{
    public class CustomerRepo : ICustomerRepo
    {
        // Reference to the Cosmos DB container used for customer operations
        private readonly Container _container;

        // Constructor to initialize the Cosmos DB container using the provided CosmosClient
        public CustomerRepo(CosmosClient client)
        {
            var database = client
                .CreateDatabaseIfNotExistsAsync("CRMDB")
                .GetAwaiter()
                .GetResult();

            _container = database.Database
                .CreateContainerIfNotExistsAsync(
                    "Customers",
                    "/id")
                .GetAwaiter()
                .GetResult()
                .Container;
        }
        // CREATE
        public async Task<Customer> AddAsync(Customer customer)
        {
            var response = await _container.CreateItemAsync(
            customer,
            new PartitionKey(customer.Id));

            return response.Resource;
        }


        //GET ALL
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c");

            var iterator = _container.GetItemQueryIterator<Customer>(query);

            List<Customer> result = new();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        //GET BY ID
        public async Task<Customer?> GetByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Customer>(
                    id,
                    new PartitionKey(id));

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // UPDATE
        public async Task<Customer?> UpdateAsync(string id, Customer customer)
        {
            customer.Id = id;

            var response = await _container.UpsertItemAsync(
                customer,
                new PartitionKey(id));

            return response.Resource;
        }


        // DELETE
        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                await _container.DeleteItemAsync<Customer>(
               id,
               new PartitionKey(id));

                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }


        //SEARCH

        public async Task<IEnumerable<Customer>> SearchAsync(string? name, string? sellerName)
        {
            var query = new QueryDefinition(@"
                SELECT * FROM c
                WHERE
                (@name = null OR CONTAINS(c.Name, @name, true))
                AND
                (@sellerName = null OR CONTAINS(c.Seller.Name, @sellerName, true))
             ")
            .WithParameter("@name", name)
            .WithParameter("@sellerName", sellerName);

            var iterator = _container.GetItemQueryIterator<Customer>(query);

            List<Customer> result = new();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }


    }
}