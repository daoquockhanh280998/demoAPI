using demoAPI.ViewModel.User;
using DemoAPI.Services.JwtManager;
using DemoAPI.Services.UserService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenAPIController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtManager _jwtManager;
        private readonly IConfiguration _config;

        public TokenAPIController(IUserService userService, IJwtManager jwtManager, IConfiguration config)
        {
            _userService = userService;
            _config = config;
            _jwtManager = jwtManager;
        }

        //[Route("checktoken")]
        //[HttpPost]
        //public IActionResult Get(string username, string password)
        //{
        //    if (CheckUser(username, password) == false)
        //    {
        //        throw new CustomException(StatusCodes.Status404NotFound, "User is NotFound");
        //    }
        //    var token = _jwtManager.GenerateToken(username);
        //    var response = Ok(new { Token = token, Message = "Success" });

        //    return response;

        //}

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            IActionResult response = Unauthorized();
            var userr = _userService.FindUserByUserName(login);
            if (userr != null)
            {
                var tokenString = _jwtManager.GenerateToken(userr);
                response = Ok(new { Token = tokenString, Message = "Success" });
            }
            return response;
        }

        [HttpGet(nameof(Get))]
        public async Task<IEnumerable<string>> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new string[] { accessToken };
        }

        //private bool ValidateToken(string token, out string username)
        //{
        //    username = null;

        //    var simplePrinciple = _jwtManager.GetPrincipal(token);
        //    var identity = simplePrinciple.Identity as ClaimsIdentity;

        //    if (identity == null)
        //        return false;

        //    if (!identity.IsAuthenticated)
        //        return false;

        //    var usernameClaim = identity.FindFirst(ClaimTypes.Name);
        //    username = usernameClaim?.Value;

        //    if (string.IsNullOrEmpty(username))
        //        return false;

        //    // More validate to check whether username exists in system

        //    return true;
        //}

        //protected Task<IPrincipal> AuthenticateJwtToken(string token)
        //{
        //    string username;

        //    if (ValidateToken(token, out username))
        //    {
        //        // based on username to get more information from database in order to build local identity
        //        var claims = new List<Claim>
        //        {
        //        new Claim(ClaimTypes.Name, username)
        //        // Add more claims if needed: Roles, ...
        //        };

        //        var identity = new ClaimsIdentity(claims, "Jwt");
        //        IPrincipal user = new ClaimsPrincipal(identity);

        //        return Task.FromResult(user);
        //    }

        //    return Task.FromResult<IPrincipal>(null);
        //}
    }
}