﻿using AutoMapper;
using Domain.Entites;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.ErrorModels;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class AuthenticationService(UserManager<User> userManager, IOptions<JwtOptions> options,IMapper mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailExist(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }
        public async Task<AddressDTO> GetUserAddress(string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
                 .FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new UserNotFoundException(email);

            return mapper.Map<AddressDTO>(user.Address);
            //return new AddressDTO
            //{
            //    City = user.Address.City,
            //    Country = user.Address.Country,
            //    FirstName = user.Address.FirstName,
            //    LastName = user.Address.LastName,

            //};
        }

        public async Task<UserResultDTO> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);
            return new UserResultDTO(user.DisplayName,
               user.Email!, await CreateTokenAsync(user));
        }

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

        public async Task<AddressDTO> UpdateUserAddress(AddressDTO address, string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
           ?? throw new UserNotFoundException(email);

            //var userAddress= mapper.Map<AddressDTO>(address);
            //user.Address = userAddress;
            if (user.Address != null)
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
            }
            else
            {
                var userAddress = mapper.Map<Address>(address);
                user.Address = userAddress;
            }

            await userManager.UpdateAsync(user);

            return address;
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
