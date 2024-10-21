using Shared.ErrorModels;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserResultDTO> LoginAsync(LoginDTO loginModel);
        public Task<UserResultDTO> RegisterAsync(UserRegisterDTO registerModel);
        //Get Current User
        public Task<UserResultDTO> GetUserByEmail(string email);

        //Check Email Exist
        public Task<bool> CheckEmailExist(string email);
        //Get User Address
        public Task<AddressDTO> GetUserAddress(string email);
        //Update User Address
        public Task<AddressDTO> UpdateUserAddress(AddressDTO address, string email);
    }
}
