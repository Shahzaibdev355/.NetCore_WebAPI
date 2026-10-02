using netcore_webapi.Dto;

namespace netcore_webapi.IServices
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync();
    }
}
