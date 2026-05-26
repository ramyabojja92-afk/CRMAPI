using CRMAPI.Entities;
using CRMAPI.Entities;

namespace CRMAPI.Interfaces
{
    public interface ICustomerRepo
    {

        // "Async" indicates the method is asynchronous and returns a Task.
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> AddAsync(Customer customer);
        Task<Customer?> GetByIdAsync(string id);
        Task<Customer?> UpdateAsync(string id, Customer customer);
        Task<bool> DeleteAsync(string id);

        Task<IEnumerable<Customer>> SearchAsync(string? name, string? sellerName);
    }
}