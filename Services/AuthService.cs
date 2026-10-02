using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using netcore_webapi.Data;
using netcore_webapi.Dto;
using netcore_webapi.IServices;

namespace netcore_webapi.Services
{
    public class AuthService(AppDbContext _context) : IAuthService
    {

        public async Task<Tuple<int, string>> LoginUser(UserDto dto)
        {
            try
            {

                var exisitingUser = await _context.AccountUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (exisitingUser == null)
                {
                    return new Tuple<int, string>(404, "User not found, Pls Register");
                }

                //if (exisitingUser.Password != dto.Password)
                //{
                //    return new Tuple<int, string>(401, "Password is incorrect");
                //}


                //return new Tuple<int, string>(200, "Login Successful");


                var passwordHasher = new PasswordHasher<string>();
                var verifyPassword = passwordHasher.VerifyHashedPassword(dto.Email, exisitingUser.Password, dto.Password);

                if (verifyPassword == PasswordVerificationResult.Success)
                {
                    return new Tuple<int, string>(200, "Login Successful");
                }

                else if (verifyPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    // Rehash the password and update it in the database

                    exisitingUser.Password = PasswordHashing(dto);
                    _context.AccountUsers.Update(exisitingUser);
                    _context.SaveChanges();

                    return new Tuple<int, string>(200, "Login Successful, new password hashed");

                }
                else if (verifyPassword == PasswordVerificationResult.Failed)
                {
                    return new Tuple<int, string>(401, "Password is incorrect");

                }

                return new Tuple<int, string>(200, "");

            }
            catch (Exception)
            {
                return new Tuple<int, string>(500, "Internal Server Error");
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




    }
}
