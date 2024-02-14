using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using ApplicationCore.Models.ViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IConfiguration _config;
        public ClientService(IUnitofWork unitofWork, IConfiguration config)
        {
            _unitofWork = unitofWork;
            _config = config;
        }

        public async Task<ServiceResponse> RegisterUserAsync(ApplicationUser registerUser)
        {
            var userRole = new UserRole();
            //Check if admin already exists
            if (registerUser.Email!.ToLower().StartsWith("admin"))
            {
                var chkIfAdminExist = await _unitofWork.UserRole.GetAsync(ur => ur.RoleName!.ToLower().Equals("admin"));
                if (chkIfAdminExist != null)
                {
                    return new ServiceResponse() { Flag = false, Message = "Admin already exists, change the email address" };
                }
                else
                {
                    userRole.RoleName = "admin";
                }
            }

            var checkIfUserAlreadyCreated = _unitofWork.ApplicationUser.Get(u => u.Email!.ToLower().Equals(registerUser.Email!.ToLower()));
            if (checkIfUserAlreadyCreated != null) return new ServiceResponse() { Flag = false, Message = $"Email: {registerUser.Email} already registered!" };

            var hashedPassword = HashPassword(registerUser.Password!);
            var newUser = new ApplicationUser
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                Password = hashedPassword,

            };
            _unitofWork.ApplicationUser.Add(newUser);
            

            if (string.IsNullOrEmpty(userRole.RoleName))
                userRole.RoleName = "Trader";

            userRole.UserId = newUser.Id;
            _unitofWork.UserRole.Add(userRole);
            _unitofWork.Commit();
            return new ServiceResponse() {Flag = true, Message = "Account Created" };

        }
        private static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(salt);
            }
            var rfcPassword = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA1);
            byte[] rfcPasswordHash = rfcPassword.GetBytes(20);

            byte[] passwordHash = new byte[36];
            Array.Copy(salt, 0, passwordHash, 0, 16);
            Array.Copy(rfcPasswordHash, 0, passwordHash, 16, 20);
            return Convert.ToBase64String(passwordHash);
        }
        public async Task<ServiceResponse> LoginUserAsync(Login loginAttempt)
        {
            var getUser = await _unitofWork.ApplicationUser.GetAsync(u => u.Email!.ToLower().Equals(loginAttempt.Email!.ToLower()));

            if (getUser == null)
            {
                return new ServiceResponse() { Flag = false, Message = "User not found" };
            }
            else // user found
            {
                if (VerifyUserPassword(loginAttempt.Password!, getUser.Password!))
                {
                    //get user role from the roles table
                    var getUserRole = await _unitofWork.UserRole.GetAsync(ur => ur.UserId == getUser.Id);

                    //Generate token with the role, and username as email

                    var token = GenerateToken(getUser.FirstName + getUser.LastName, getUser.Email, getUserRole.RoleName!);

                    var checkIfTokenExists = _unitofWork.TokenInfo.Get(token => token.UserId == getUser.Id);

                    if (checkIfTokenExists == null)
                    {
                        _unitofWork.TokenInfo.Add(new TokenInfo() { Token = token, UserId = getUser.Id });
                        _unitofWork.Commit();
                        return new ServiceResponse() { Flag = true, Token = token };
                    }
                    checkIfTokenExists.Token = token;

                    _unitofWork.Commit();
                    return new ServiceResponse() { Flag = true, Token = token };
                }
                return new ServiceResponse() { Flag = false, Message = "Invalid email or password" };
            }
        }
        //decrypt user database password
        private static bool VerifyUserPassword(string rawPassword, string databasePassword)
        {
            byte[] dbPasswordHash = Convert.FromBase64String(databasePassword);
            byte[] salt = new byte[16];
            Array.Copy(dbPasswordHash, 0, salt, 0, 16);
            var rfcPassword = new Rfc2898DeriveBytes(rawPassword, salt, 1000, HashAlgorithmName.SHA1);
            byte[] rfcPasswordHash = rfcPassword.GetBytes(20);
            for(int i =0; i<rfcPasswordHash.Length; i++)
            {
                if (dbPasswordHash[i + 16] != rfcPasswordHash[i])
                    return false;
            }
            return true;
        }
        private string GenerateToken(string name, string email, string roleName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
