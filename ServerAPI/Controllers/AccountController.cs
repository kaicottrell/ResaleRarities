using ApplicationCore.Models;
using ApplicationCore.Models.NonDbModels;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IClientService service;
        public AccountController(IClientService service)
        {
            this.service = service;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse>> RegisterUserAsync(ApplicationUser model)
        {
            if (model is null) return BadRequest("Model is empty");
            var result = await service.RegisterUserAsync(model);
            return Ok(result);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse>> LoginUserAsync(Login model)
        {
            if (model is null) return BadRequest();
            var result = await service.LoginUserAsync(model);
            return Ok(result);
        }
    }
}