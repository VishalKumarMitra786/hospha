using Hospha.DbModel;
using Hospha.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospha.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = Hospha.Model.Constants.Roles.admin)]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            var userDb = await _userService.AddUser(user);
            return CreatedAtAction(nameof(AddUser), new { id = userDb.Id }, user);
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUsers()
        {
            var usersDb = await _userService.GetUsers();
            return Ok(usersDb);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var userDb = await _userService.GetUserByEmail(email);
            return Ok(userDb);
        }

        [HttpGet("phone/{phone}")]
        public async Task<ActionResult<User>> GetUserByPhone(string phone)
        {
            var userDb = await _userService.GetUserByPhoneNumber(phone);
            return Ok(userDb);
        }
    }
}
