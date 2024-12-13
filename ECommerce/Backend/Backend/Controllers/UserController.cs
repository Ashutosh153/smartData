using App.Core.App.Command;
using App.Core.App.Query;
using App.Core.Model;
using Domain.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Regester")]

        public async Task<IActionResult> DoCreateUser(RegestrationDto regestration)
        {
            var result = await _mediator.Send(new CreateUserCommand { newUser = regestration });
            return Ok(result);
        }

        [HttpPost("sendOtp")]

        public async Task<IActionResult> DoLogin(LoginDto loginData)
        {
            var result = await _mediator.Send(new LoginQuery { _loginDetails = loginData });
            return Ok(result);
        }
        [HttpPost("forgetPassword")]
        public async Task<IActionResult> DoForgetPassword(string email)
        {
            var result = await _mediator.Send(new ForgetPasswordCommand { Email = email });
            return Ok(result);
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> DoUpdateUser(UpdateUserDto updateUser)
        {
            var result = await _mediator.Send(new UpdateUserCommand { UpdateUserValue = updateUser });
            return Ok(result);
        }

        [HttpPost("varifyOtpAndLogin")]
        public async Task<IActionResult> DoVerifyOtp(Otp otp)
        {
            var result = await _mediator.Send(new VerifyOtpQuery { otpDetails = otp });
            return Ok(result);
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> DoAddProduct(ProductDto product)
        {
            var result = await _mediator.Send(new AddProductCommand { NewProduct = product });
            return Ok(result);
        }

        [HttpGet("getAllProducts/{id}")]
       // [Authorize]
        public async Task<IActionResult> DoGetAllProducts(int id)
        {
            var result = await _mediator.Send(new GetAllProductQuery() { Id=id});
            return Ok(result);
        }

        [HttpGet("getUserById/{id}")]
        public async Task<IActionResult> DoGetUserById(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
            return Ok(result);
        }
        [HttpPost("uploadFile")]
        public async Task<IActionResult> DoUploadFile(IFormFile file)
        {
            var result = await _mediator.Send(new SaveImageCommand { imageFile = file });
            return Ok(result);
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> DoAddToCaet(AddToCartDto cartValue)
        {
            var result = await _mediator.Send(new AddToCartCommand { AddToCard = cartValue });
            return Ok(result);
        }

        [HttpGet("getProductsFromCart")]
        public async Task<IActionResult> DoGetAllCartProducts(int Id)
        {
            var result = await _mediator.Send(new GetAllCartProductQuery { userId = Id });
            return Ok(result);
        }
        [HttpPost("removeFromCart")]
        public async Task<IActionResult> DoRemoveFromCart(RemoveFromCartDto removeProduct)
        {
            var result = await _mediator.Send(new RemoveItemFromCartCommand { cartDto = removeProduct });
            return Ok(result);
        }
        [HttpGet("getAllCountries")]
        public async Task<IActionResult> DoGetAllCountries()
        {
            var result = await _mediator.Send(new GetAllCountriesQuery());
            return Ok(result);
        }
        [HttpGet("getAllStates/{id}")]
        public async Task<IActionResult> DoGetAllStates(int id)
        {
            var result = await _mediator.Send(new GetAllStetesQuery { Id = id });
            return Ok(result);
        }
        [HttpDelete("doDeleteAllProduct/{id}")]
        public async Task<IActionResult> DoDeletePRoduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand { id = id });
            return Ok(result);
        }
        [HttpPost("doEditProduct")]
        public async Task<IActionResult> DoUpdateProduct(ProductDto product)
        {
            var result = await _mediator.Send(new UpdateProductCommand { Product = product });
            return Ok(result);
        }
        [HttpPost("incrementTheQuantity")]
        public async Task<IActionResult> DoIncrementQuantity(IncDecObjDto incdecobj)
        {
            var result = await _mediator.Send(new IncrementQuantitiCommand { IncDecVal = incdecobj });
            return Ok(result);
        }
        [HttpPost("decrementTheQuantity")]
        public async Task<IActionResult> DoDecrementData(IncDecObjDto incDecObjDto)
        {
            var result = await _mediator.Send(new DecrementQuantitiCommand { IncDecObj = incDecObjDto });
            return Ok(result);
        }

        [HttpPost("addDataToSalesMaster")]
        public async Task<IActionResult> DoAddDataToSalesMaster(SalesMasterDto salesMasterDto)
        {
            var result=await _mediator.Send(new AddToSalesMasterCommand { SalesMasterVal = salesMasterDto });
            return Ok(result);
        }
        [HttpPost("validateCardDetails")]
        public async Task<IActionResult> DoValidateCardDetails(CardDetailsDto cardDetailsDto)
        {
            var result = await _mediator.Send(new ValidateCardDetailsQuery { CardDetails = cardDetailsDto });
            return Ok(result);
        }
        [HttpPost("getAllInvoiceData")]
        public async Task<IActionResult> DoGetAllInvoiceData(string invoiceId)
        {
            var result=await _mediator.Send(new GetInvoiceDEtailsQuerry { invoiceId = invoiceId });
            return Ok(result);
        }
        [HttpGet("getCartItemQuantity/{userId}")]
        public async Task<IActionResult> DoGetCartCount(int userId)
        {
            var result = await _mediator.Send(new GetCartQuantityQuerry { userId = userId });
            return Ok(result);
        }
        [HttpPost("chnageUserPassword")]
        public async Task<IActionResult> DoChangeUserPassword(ChangePasswordCto changePass)
        {
            var result=await _mediator.Send(new ChangePasswordCommand { chnagePassObj = changePass });
            return Ok(result);  
        }
    }
}
