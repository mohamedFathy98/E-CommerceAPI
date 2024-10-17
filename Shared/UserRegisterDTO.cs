using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record UserRegisterDTO
    {
        [Required(ErrorMessage = "DisPlayName is Required")]
        public string DisPlayName { get; init; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; init; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; init; }
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; init; }

        public string? PhoneNumber { get; init; }

    }
}
