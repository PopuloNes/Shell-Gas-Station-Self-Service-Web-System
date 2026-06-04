using gsst.Interfaces;
using gsst.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace PetrolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentMethodController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPaymentMethods(int userId)
        {
            try
            {
                var methods = _paymentService.GetPaymentMethodsForUser(userId);
                return Ok(methods);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public class AddPaymentMethodRequest
        {
            public int UserId { get; set; }
            public string Type { get; set; }
            public string Details { get; set; }
            public bool IsDefault { get; set; }
        }

        [HttpPost]
        public IActionResult AddPaymentMethod([FromBody] AddPaymentMethodRequest req)
        {
            try
            {
                var method = _paymentService.AddPaymentMethod(req.UserId, req.Type, req.Details, req.IsDefault);
                return Ok(method);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{methodId}")]
        public IActionResult DeletePaymentMethod(int methodId)
        {
            try
            {
                _paymentService.RemovePaymentMethod(methodId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
