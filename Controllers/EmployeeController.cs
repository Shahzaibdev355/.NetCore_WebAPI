using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netcore_webapi.Dto;
using netcore_webapi.GenericResponse;
using netcore_webapi.IServices;

namespace netcore_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployeeAsync()
        {
            try
            {
                var result = await employeeService.GetAllEmployeeAsync();

                if (!result.Item2.Any())
                {
                    return Ok(ResponseResult<List<EmployeeDto>>.Failure(null, "No employees found."));
                }

                return Ok(ResponseResult<List<EmployeeDto>>.Success(result.Item2, "Employees retrieved successfully."));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
