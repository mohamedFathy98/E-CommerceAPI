using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.ErrorModels;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
