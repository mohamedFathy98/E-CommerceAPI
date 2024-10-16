using Domain.Entites;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class AuthenticationService(UserManager<User> userManager) : IAuthenticationService
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
                 user.Email!, "Token");

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
                // throw new Domain.Exceptions.ValidationException(errors);

            }
            return new UserResultDTO(user.DisplayName,
                user.Email!, "Token");
        }
    }
    }
