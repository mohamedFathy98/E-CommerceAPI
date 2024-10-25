using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.ErrorModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shared.OrderModels;
using System.Security.Claims;

namespace Presentation
{
    public class AuthenticationController(IServicesManger serviceManager) : ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDTO>> Login(LoginDTO loginDTO)
        {
            var result = await serviceManager.AuthenticationService.LoginAsync(loginDTO);
            return Ok(result);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDTO>> Register(UserRegisterDTO userRegisterDTO)
        {
            var result = await serviceManager.AuthenticationService.RegisterAsync(userRegisterDTO);
            return Ok(result);
        }
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string eamil)
        {
            return Ok(await serviceManager.AuthenticationService.CheckEmailExist(eamil));
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.AuthenticationService.GetUserByEmail(email);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetAdress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.AuthenticationService.GetUserByEmail(email);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.AuthenticationService.UpdateUserAddress(address, email);
            return Ok(result);
        }
    }
}
