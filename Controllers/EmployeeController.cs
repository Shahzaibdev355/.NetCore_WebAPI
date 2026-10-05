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


        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody]EmployeeDto employeeDto)
        {
            try
            {

                var result = await employeeService.CreateEmployee(employeeDto);

                if(result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch (Exception)
            {
                throw;
            }
        }





        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
            try
            {

                var result = await employeeService.UpdateEmployee(employeeDto);

                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {

                var result = await employeeService.DeleteEmployee(id);

                if(result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {

                var result = await employeeService.GetEmployeeById(id);
                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<EmployeeDto>.Failure(null, "Employee not found."));
                }
                return Ok(ResponseResult<EmployeeDto>.Success(result.Item2, "Employee retrieved successfully."));

            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
