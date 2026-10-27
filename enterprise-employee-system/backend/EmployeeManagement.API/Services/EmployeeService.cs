using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();
            return employees.Select(MapToDto);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return employee is null ? null : MapToDto(employee);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            // Business validations
            if (dto.DateOfJoining > System.DateTime.UtcNow)
                throw new ArgumentException("DateOfJoining cannot be in the future.");

            if (dto.Salary < 0)
                throw new ArgumentException("Salary must be non-negative.");

            var existingByEmail = await _repository.GetByEmailAsync(dto.Email);
            if (existingByEmail is not null)
                throw new ArgumentException("An employee with the provided email already exists.");

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Department = dto.Department,
                Salary = dto.Salary,
                DateOfJoining = dto.DateOfJoining,
                IsActive = true,
                CreatedDate = System.DateTime.UtcNow
            };

            var created = await _repository.AddAsync(employee);
            return MapToDto(created);
        }

        public async Task<bool> UpdateAsync(UpdateEmployeeDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing is null)
                return false;

            // Business validations
            if (dto.DateOfJoining > System.DateTime.UtcNow)
                throw new ArgumentException("DateOfJoining cannot be in the future.");

            if (dto.Salary < 0)
                throw new ArgumentException("Salary must be non-negative.");

            if (!string.Equals(existing.Email, dto.Email, System.StringComparison.OrdinalIgnoreCase))
            {
                var otherWithEmail = await _repository.GetByEmailAsync(dto.Email);
                if (otherWithEmail is not null && otherWithEmail.Id != dto.Id)
                    throw new ArgumentException("Another employee with the provided email already exists.");
            }

            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.Department = dto.Department;
            existing.Salary = dto.Salary;
            existing.DateOfJoining = dto.DateOfJoining;
            existing.IsActive = dto.IsActive;
            existing.ModifiedDate = System.DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return false;

            await _repository.DeleteAsync(existing);
            return true;
        }

        private static EmployeeDto MapToDto(Employee e) => new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Department = e.Department,
            Salary = e.Salary,
            DateOfJoining = e.DateOfJoining,
            IsActive = e.IsActive,
            CreatedDate = e.CreatedDate,
            ModifiedDate = e.ModifiedDate
        };
    }
}
