using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class PaymentsController(IServicesManger serviceManager)
         : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var result = await serviceManager.paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(result);
        }
        [HttpPost("webhook")]
        public async Task<ActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await serviceManager.paymentService.UpdateOrderPaymentStatus(json, Request.Headers[
                "Stripe-Signature"]!);

            return new EmptyResult();
        }
    }
}
