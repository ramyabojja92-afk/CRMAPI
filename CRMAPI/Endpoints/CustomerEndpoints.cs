using CRMAPI.Entities;
using CRMAPI.Interfaces;
using CRMAPI.Entities;
using CRMAPI.Interfaces;
using System.Runtime.CompilerServices;

namespace CRMAPI.Endpoints
{
    public static class CustomerEndpoints
    {
        // Extension method to register all customer endpoints
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            // Create endpoint group with base route and tag
            var group = app.MapGroup("/api/customers").WithTags("Customers");

            group.MapGet("/", GetAllCustomers);
            group.MapGet("/search", SearchCustomers);
            group.MapGet("/{id}", GetCustomerById);
            group.MapPost("/", AddCustomer);
            group.MapPut("/{id}", UpdateCustomer);
            group.MapDelete("/{id}", DeleteCustomer);

        }
        private static async Task<IResult> GetAllCustomers(ICustomerRepo repo)
        {
            var customers = await repo.GetAllAsync();
            return Results.Ok(customers);
        }

        private static async Task<IResult> SearchCustomers(string? name, string? sellerName, ICustomerRepo repo)
        {
            var customers = await repo.SearchAsync(name, sellerName);
            return Results.Ok(customers);
        }

        private static async Task<IResult> GetCustomerById(string id, ICustomerRepo repo)
        {
            var customer = await repo.GetByIdAsync(id);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        }

        private static async Task<IResult> AddCustomer(Customer customer, ICustomerRepo repo)
        {
            var createdCustomer = await repo.AddAsync(customer);
            return Results.Created($"/api/customers/{createdCustomer.Id}", createdCustomer);
        }

        private static async Task<IResult> UpdateCustomer(string id, Customer customer, ICustomerRepo repo)
        {
            var updatedCustomer = await repo.UpdateAsync(id, customer);
            return updatedCustomer is not null ? Results.Ok(updatedCustomer) : Results.NotFound(); // Return updated customer or 404 if not found
        }

        private static async Task<IResult> DeleteCustomer(string id, ICustomerRepo repo)
        {
            var deleted = await repo.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();// Return 204 if deleted, otherwise 404
        }
    }
}