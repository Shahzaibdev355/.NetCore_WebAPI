using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netcore_webapi.Dto;
using netcore_webapi.GenericResponse;
using netcore_webapi.IServices;


namespace netcore_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // primary constructor approach
    public class AuthController(IAuthService authService) : ControllerBase
    {

        private readonly IAuthService _authService = authService;

        // general constructor approach
        //public AuthController(IAuthService authService)
        //{
        //    _authService = authService;
        //}

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            try
            {
                var result = await _authService.LoginUser(userDto);

                if (result.Item1 == 404)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }
                if (result.Item1 == 401)
                {
                    return Unauthorized(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDto userDto)
        {
            try
            {

                var result = await _authService.RegisterUser(userDto);
                if(result.Item1 == 409)
                {
                    return Conflict(ResponseResult<string>.Failure(null, result.Item2));
                }

              

                return Ok(ResponseResult<string>.Success(null, result.Item2));


            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
