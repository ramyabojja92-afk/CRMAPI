using CRMAPI.Entities;

namespace CRMAPI.Interfaces
{
    public interface ICustomerRepo
    {
        Task<List<Customer>> GetAllAsync();

        Task<Customer?> GetByIdAsync(string id);

        Task<Customer> AddAsync(Customer customer);

        Task UpdateAsync(Customer customer);

        Task DeleteAsync(string id);
    }
}