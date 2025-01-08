using System.Security.Claims;
using Artico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Artico.Models;
using Artico.DbModels;

namespace Artico.Controllers.AccountController
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;
        public AccountController(AppDbContext context, JwtTokenService jwtTokenService) { 
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost, Route("login")]
        public IActionResult Login(LoginModel model)
        {
            Users? user = _context.users.FirstOrDefault(u => u.email == model.email && u.password == model.password);
            if (user != null) {
                var token = _jwtTokenService.GenerateToken(user.id, user.username, user.email);
                return Ok(new { user = new UserModel { username = user.username, email = user.email, token = token } });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet, Route("is-authenticated")]
        public IActionResult IsAuthenticated()
        {
            return Ok(new { isauth = true });
        }

        [Authorize]
        [HttpGet, Route("get-current-user")]
        public IActionResult GetCurrentUsr()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                string? username = User.FindFirst(ClaimTypes.Name)?.Value;
                string? email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (username is not null && email is not null)
                {
                    var token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                    return Ok(new
                    {
                        user =
                        new UserModel
                        {
                            username = username,
                            email = email,
                            token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length)
                        }
                    });
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
    }
}
