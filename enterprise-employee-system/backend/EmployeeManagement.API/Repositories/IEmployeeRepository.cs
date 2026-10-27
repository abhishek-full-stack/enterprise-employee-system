using EmployeeManagement.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee?> GetByEmailAsync(string email);
        Task<Employee> AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);
    }
}
