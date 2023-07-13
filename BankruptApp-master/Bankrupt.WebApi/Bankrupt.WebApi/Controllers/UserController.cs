using Bankrupt.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;
using Bankrupt.WebApi.Dto;
using Bankrupt.Core.Services;

namespace Bankrupt.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [Route("All")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetAllUsers();
            return Ok(users.Select(u => u.ToDto()));
        }

        [HttpGet]
        //[Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await userService.GetUser(id);
            return Ok(user.ToDto());
        }

        [HttpDelete]
        //[Authorize]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await userService.DeleteUserById(id);
            return Ok();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
        {
            var newUserId = await userService.AddUser(user.ToEntity());     
            return Ok(newUserId);
        }

        [HttpPost]
        //[Authorize]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto user)
        {
            await userService.UpdateUser(user.ToEntity());
            return Ok();
        }
    }
}