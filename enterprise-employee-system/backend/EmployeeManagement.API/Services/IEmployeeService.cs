using EmployeeManagement.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task<bool> UpdateAsync(UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
