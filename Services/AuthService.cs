using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using netcore_webapi.Data;
using netcore_webapi.Dto;
using netcore_webapi.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace netcore_webapi.Services
{
    public class AuthService(
             AppDbContext _context,
             IHttpContextAccessor _httpContextAccessor
         ) : IAuthService
    {

        public async Task<Tuple<int, string>> LoginUser(UserDto dto)
        {
            try
            {
                var existingUser = await _context.AccountUsers
                    .FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (existingUser == null)
                {
                    return new Tuple<int, string>(
                        404,
                        "User not found, Please Register"
                    );
                }

                var passwordHasher = new PasswordHasher<string>();

                var verifyPassword = passwordHasher.VerifyHashedPassword(
                    dto.Email,
                    existingUser.Password,
                    dto.Password
                );

                if (verifyPassword == PasswordVerificationResult.Failed)
                {
                    return new Tuple<int, string>(
                        401,
                        "Password is incorrect"
                    );
                }

                // Rehash if required
                if (verifyPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    existingUser.Password = PasswordHashing(dto);

                    _context.AccountUsers.Update(existingUser);

                    await _context.SaveChangesAsync();
                }

                // Generate JWT
                var token = GenerateJwtToken(existingUser);

                // Store JWT in HttpOnly cookie
                _httpContextAccessor.HttpContext!.Response.Cookies.Append(
                    "jwt_key",
                    token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                    }
                );

                return new Tuple<int, string>(
                    200,
                    "Login Successful"
                );
            }
            catch (Exception)
            {
                return new Tuple<int, string>(
                    500,
                    "Internal Server Error"
                );
            }
        }



        public async Task<Tuple<int, string>> RegisterUser(UserDto dto)
        {

            try
            {
                var exisitingUser = await _context.AccountUsers.AnyAsync(x => x.Email == dto.Email);

                if (exisitingUser)
                {
                    return new Tuple<int, string>(409, "User already exists, Pls Register with new User");
                }

                _context.AccountUsers.Add(new Entities.User
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = PasswordHashing(dto)
                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(201, "User Registered Successfully");

            }
            catch (Exception)
            {
                throw;
            }

        }


        private string PasswordHashing(UserDto dto)
        {
            var passwordHasher = new PasswordHasher<string>();
            var hash = passwordHasher.HashPassword(dto.Email, dto.Password);

            return hash;
        }



        private string GenerateJwtToken(Entities.User user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(
                "1JP6JfDJJ9CMh0tXjOlioUH4Cm69QYmCtjl6xBhWeZS"
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        user.Id.ToString()
                    ),

                    new Claim(ClaimTypes.Name, user.Name),

                    new Claim(ClaimTypes.Email,user.Email)
                }),

                Expires = DateTime.UtcNow.AddMinutes(30),

                Issuer = "shahzaib-client",

                Audience = "shahzaib-backend",

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var token = jwtHandler.CreateToken(tokenDescriptor);

            return jwtHandler.WriteToken(token);
        }



    }
}
