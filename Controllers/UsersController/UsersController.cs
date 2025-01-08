using Artico.DbModels;
using Artico.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Artico.Models;

namespace Artico.Controllers.UsersController
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;
        
        public UsersController(AppDbContext context, JwtTokenService jwtTokenService) {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id) {
            Users? user = await _context.users.FirstOrDefaultAsync(u=>u.id==id);
            if (user == null) {
                return BadRequest("User does'nt exists.");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsersPost user)
        {
            Users? userExists = await _context.users.FirstOrDefaultAsync(u => u.email == user.email);

            if (userExists is null)
            {
                Users userDetails = new Users
                {
                    username=user.username,
                    email=user.email,
                    password=user.password,
                };
                _context.users.Add(userDetails);
            
                await _context.SaveChangesAsync();

                var token = _jwtTokenService.GenerateToken(userDetails.id, user.username, user.email);

                return CreatedAtAction(nameof(Register), new { id = userDetails.id }, new { user = new UserModel{ username = user.username, email = user.email, token = token } });
            }
            return Conflict("User already exists.");
        }
    }
}
