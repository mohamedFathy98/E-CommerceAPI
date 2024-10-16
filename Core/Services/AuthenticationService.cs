using Domain.Entites;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class AuthenticationService(UserManager<User> userManager, IOptions<JwtOptions> options) : IAuthenticationService
    {
        public async Task<UserResultDTO> LoginAsync(LoginDTO loginModel)
        {
            // Check if there is user under this email
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user == null) throw new UnAuthorizeException("Email Does't Exist");
            // check if the paasword is crrocet for this email
            var result = await userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!result) throw new UnAuthorizeException();
            //create token and return response
            return new UserResultDTO(user.DisplayName,
                 user.Email!, await CreateTokenAsync(user));

        }

        public async Task<UserResultDTO> RegisterAsync(UserRegisterDTO registerModel)
        {
           
            var user = new User()
            {
                Email = registerModel.Email,
                DisplayName = registerModel.DisPlayName,
                PhoneNumber = registerModel.PhoneNumber,
                UserName = registerModel.UserName,
            };
            var result = await userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);

            }
            return new UserResultDTO(user.DisplayName,
                user.Email!, await CreateTokenAsync(user));
        }
        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOption = options.Value;
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                 new Claim(ClaimTypes.Email,user.Email)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                audience: jwtOption.Audience,
                issuer: jwtOption.Issure,
                expires: DateTime.UtcNow.AddDays(jwtOption.DurationInDays),
                claims: authClaims,
                signingCredentials: signingCreds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    }
