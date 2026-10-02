using Microsoft.EntityFrameworkCore;
using netcore_webapi.Data;
using netcore_webapi.Dto;
using netcore_webapi.IServices;

namespace netcore_webapi.Services
{
    public class EmployeeService(AppDbContext _context) : IEmployeeService 
    {

        public async Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync()
        {
            try
            {
                var employees = await _context.Employees
                    .AsNoTracking()
                    .ToListAsync();
                var employeeDtos = employees.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    CreatedDate = e.CreatedDate,
                    LastModifiedDate = e.LastModifiedDate,
                    DOB = e.DOB,
                    Position = e.Position,
                    Department = e.Department,
                    EmailAddress = e.EmailAddress
                }).ToList();
                return new Tuple<int, List<EmployeeDto>>(200, employeeDtos);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
