using Microsoft.EntityFrameworkCore;
using netcore_webapi.Data;
using netcore_webapi.Dto;
using netcore_webapi.Entities;
using netcore_webapi.IServices;

namespace netcore_webapi.Services
{
    public class EmployeeService(AppDbContext _context) : IEmployeeService
    {

        // get employee
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


        public async Task<Tuple<int, string>> CreateEmployee(EmployeeDto employee)
        {
            try
            {

                var existing = await _context.Employees.AnyAsync(x => x.EmailAddress == employee.EmailAddress);

                if (existing)
                {
                    return new Tuple<int, string>(409, "Employee Already exist with same Email Id");

                }

                await _context.Employees.AddAsync(new Entities.Employee
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    LastModifiedDate = null,
                    Name = employee.Name,
                    DOB = employee.DOB,
                    Position = employee.Position,
                    Department = employee.Department,
                    EmailAddress = employee.EmailAddress
                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(201, "Employee Created Successfully");

            }
            catch (Exception)
            {
                throw;
            }
        }





        public async Task<Tuple<int, string>> UpdateEmployee(EmployeeDto employee)
        {
            try
            {

                if(employee == null)
                {
                    return new Tuple<int, string>(404, "Pls fill all required details");

                }


                var existing = await _context.Employees.FirstOrDefaultAsync(x => x.EmailAddress == employee.EmailAddress);

                if (existing == null)
                {
                    return new Tuple<int, string>(404, "Employee not found with the given Email Id");
                }

                existing.Position = string.IsNullOrWhiteSpace(employee.Position) ? existing.Position : employee.Position;
                existing.DOB = employee.DOB ?? existing.DOB;
                existing.Name = string.IsNullOrWhiteSpace(employee.Name) ? existing.Name : employee.Name;
                existing.Department = string.IsNullOrWhiteSpace(employee.Department) ? existing.Department : employee.Department;
                existing.EmailAddress = employee.EmailAddress;
                existing.LastModifiedDate = DateTime.UtcNow;

                _context.Employees.Update(existing);
                await _context.SaveChangesAsync();

                return new Tuple<int, string>(201, "Employee Updated Successfully");

            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<Tuple<int, string>> DeleteEmployee(Guid id)
        {
            var data = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
            {
                return new Tuple<int, string>(404, "Employee not found with the given Email Id");
            }

            _context.Employees.Remove(data);
            await _context.SaveChangesAsync();

            return new Tuple<int, string>(200, "Employee Deleted Successfully");
        }







    }
}
