using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using System.Net;

namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationResponse(ActionContext context)
        {
            //get all errors in model state
            var error = context.ModelState.Where(error => error.Value.Errors.Any())
                .Select(error => new ValidationError
                {
                    Filed = error.Key,
                    Errors = error.Value.Errors.Select(e => e.ErrorMessage)
                });
            //create custom respons
            var respone = new ValidationErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = "Validation Filed",
                Errors = error

            };
            //return
            return new BadRequestObjectResult(respone);
        }
    }
}
