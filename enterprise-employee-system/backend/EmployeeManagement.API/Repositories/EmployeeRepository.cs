using EmployeeManagement.API.Data;
using EmployeeManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
