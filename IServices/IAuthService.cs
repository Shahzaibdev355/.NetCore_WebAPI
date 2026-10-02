using netcore_webapi.Dto;

namespace netcore_webapi.IServices
{
    public interface IAuthService
    {
        Task<Tuple<int, string>> LoginUser(UserDto dto);
        Task<Tuple<int, string>> RegisterUser(UserDto dto);
    }
}
