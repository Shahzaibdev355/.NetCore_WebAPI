using netcore_webapi.Dto;

namespace netcore_webapi.IServices
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync();
        Task<Tuple<int, string>> CreateEmployee(EmployeeDto employee);

        Task<Tuple<int, string>> UpdateEmployee(EmployeeDto employee);

        Task<Tuple<int, string>> DeleteEmployee(Guid id);
    }
}
