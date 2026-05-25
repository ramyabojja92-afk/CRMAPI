using CRMAPI.Entities;
using CRMAPI.Interfaces;

namespace CRMAPI.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/customers", async (ICustomerRepo repo) =>
            {
                return await repo.GetAllAsync();
            });

            app.MapGet("/api/customers/{id}", async (string id, ICustomerRepo repo) =>
            {
                var customer = await repo.GetByIdAsync(id);

                return customer is not null
                    ? Results.Ok(customer)
                    : Results.NotFound();
            });

            app.MapPost("/api/customers", async (Customer customer, ICustomerRepo repo) =>
            {
                await repo.AddAsync(customer);

                return Results.Ok(customer);
            });

            app.MapPut("/api/customers/{id}", async (
                string id,
                Customer customer,
                ICustomerRepo repo) =>
            {
                customer.Id = id;

                await repo.UpdateAsync(customer);

                return Results.Ok(customer);
            });

            app.MapDelete("/api/customers/{id}", async (
                string id,
                ICustomerRepo repo) =>
            {
                await repo.DeleteAsync(id);

                return Results.Ok();
            });
        }
    }
}